using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders.Physical;
using ReInject.Interfaces;
using ReInject.PostInjectors.BackgroundWorker;
using ReInject.PostInjectors.EventInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TamagitchiClient.Database;
using TamagitchiClient.GitConnector.Models;
using TamagitchiClient.GPT3Prompts;
using TamagitchiClient.TamagotchiLogic.Models;

namespace TamagitchiClient.TamagotchiLogic
{
  public class TamagotchiCore : IDisposable
  {
    private TimeSpan StarvationRate { get; init; }
    private TimeSpan MaxDisplayUpdateAge { get; init; }
    private readonly IDependencyContainer _container;

    private Thread _workerThread;
    private readonly ConcurrentBag<CommitEvent> _events = new ConcurrentBag<CommitEvent>();
    private readonly ConcurrentQueue<DisplayUpdate> _displayUpdates = new ConcurrentQueue<DisplayUpdate>();
    private AutoResetEvent runLoop = new AutoResetEvent(false);
    private PromptGenerator _promptGen;
    private volatile bool _running = true;
    private long _userId;

    public TamagotchiCore(IDependencyContainer container, PromptGenerator promptGen)
    {
      _container = container;
      var config = container.GetInstance<IConfiguration>();
      StarvationRate = config.GetValue<TimeSpan>("Settings:StarvationRate");
      MaxDisplayUpdateAge = config.GetValue<TimeSpan>("Settings:MaxDisplayAge");
      _userId = config.GetValue<long>("Settings:GitlabUserId");
      _workerThread = new Thread(workerThread);
      _workerThread.Start();
      _promptGen=promptGen;
    }

    public bool TryGetNextUpdate(out DisplayUpdate update)
    {
      do
      {
        if (_displayUpdates.TryDequeue(out update) == false)
          return false;
      } while (update != null && update.Timestamp - DateTime.UtcNow > MaxDisplayUpdateAge);
      return update != null;
    }


    [InjectEvent("GitConnector:OnUserCommit")]
    private void handleCommit(CommitEvent commit)
    {
      if (commit.User.GitlabId != _userId)
        return;

      _events.Add(commit);
      runLoop.Set();
    }

    [BackgroundWorker(Schedule = "0 0/2 * * *")]
    private void checkStarvation()
    {
      runLoop.Set();
    }

    private async void workerThread()
    {
      while (_running)
      {
        if(_events.IsEmpty)
          runLoop.WaitOne();
        
        if (_running == false)
          return;

        using (var context = _container.GetInstance<TamagitchiContext>())
        {
          var pets = await context.Pets.Where(x => x.Alive).ToListAsync();
          var petLookup = pets.ToDictionary(x => x.Owner.Id, x => x);
          List<CommitEvent> copy = new List<CommitEvent>();
          while (_events.TryTake(out var commit))
            copy.Add(commit);
          
          copy = copy.OrderBy(x => x.Timestamp).ToList();
          var now = DateTime.UtcNow;
          foreach (var commit in copy)
          {
            var pet = petLookup[commit.User.Id];
            var food = Math.Max(commit.Diffs
              .SelectMany(x => x.Chunks)
              .Sum(x => x.AfterLineCount) / 25, 1);

            var before = pet.CurrentHealth;
            pet.LastFood = now;
            pet.LastStarvationTick = null;
            var newHealth = Math.Min(pet.MaxHealth, pet.CurrentHealth + food);
            var added = commit.Diffs.Count(x => x.Added);
            var deleted = commit.Diffs.Count(x => x.Deleted);
            var changes = commit.Diffs.Sum(x => x.Chunks.Sum(c => c.AfterLineCount));
            var request = new GenerateTextRequest(pet, newHealth, $"Commit: {added} files added, {deleted} files removed, {changes} places changed");
            pet.CurrentHealth = newHealth;
            var text = await _promptGen.GenerateTextAsync(request);
            var update = new DisplayUpdate { Timestamp = now, Pet = pet, Text = text, Animation = "headpat" };
            _displayUpdates.Enqueue(update);
          }

          foreach (var pet in pets.Where(x => now - x.LastFood > StarvationRate && x.LastStarvationTick.HasValue == false))
          {
            pet.LastStarvationTick = now;
            var request = new GenerateTextRequest(pet, pet.CurrentHealth - 1, $"Tell Developer Code More, no commit since {(int)StarvationRate.TotalHours}h");
            pet.CurrentHealth--;
            if (pet.CurrentHealth == 0)
              pet.Alive = false;
            var text = await _promptGen.GenerateTextAsync(request);
            var update = new DisplayUpdate { Timestamp = now, Pet = pet, Text = text, Animation = null };
            _displayUpdates.Enqueue(update);
          }

          foreach (var pet in pets.Where(x => x.LastStarvationTick.HasValue && now - x.LastStarvationTick.Value > StarvationRate))
          {
            var diff = now - pet.LastStarvationTick.Value;
            int lostHealth = (int)Math.Floor(diff / StarvationRate);
            var newHealth = Math.Min(0, pet.CurrentHealth - lostHealth);
            var request = new GenerateTextRequest(pet, newHealth, $"No commit since {(int)diff.Days}d, {(int)diff.Hours}h");
            pet.CurrentHealth = newHealth;
            pet.LastStarvationTick += (lostHealth * StarvationRate);
            if (pet.CurrentHealth == 0)
              pet.Alive = false;
            var text = await _promptGen.GenerateTextAsync(request);
            var update = new DisplayUpdate { Timestamp = now, Pet = pet, Text = text, Animation = null };
            _displayUpdates.Enqueue(update);
          }

          await context.SaveChangesAsync();
        }
      }
    }

    public void Dispose()
    {
      _running = false;
      runLoop.Set();
      if (_workerThread.Join(5000))
      {
        runLoop.Dispose();
      }
    }
  }
}
