using EmbedIO;
using EmbedIO.Net;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using GitLabApiClient.Models.Webhooks.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReInject.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TamagitchiClient.Database;
using TamagitchiClient.Database.Models;
using TamagitchiClient.GitConnector.Clients.Gitlab;
using TamagitchiClient.GitConnector.Models;

namespace TamagitchiClient.GitConnector
{
  public class GitConnector : IDisposable
  {
    private WebServer _webServer;
    private IDependencyContainer _container;
    private IConfiguration _config;

    public event Action<TamagitchiUser> OnUserCreated;
    public event Action<CommitEvent> OnUserCommit;
    public event Action<TamagitchiProject> OnProjectCreated;

    public GitConnector(IDependencyContainer container)
    {
      _container = container;
      _config = container.GetInstance<IConfiguration>();
      var section = _config.GetSection("CallbackService");
      _webServer = new WebServer(o => o
        .WithUrlPrefix(_config.GetHostUrl())
        .WithUrlPrefix(_config.GetExternalUrl())
        .WithMode(HttpListenerMode.EmbedIO))
        .WithWebApi("/api", api =>
        {
          api.OnUnhandledException += (c, e) => { Debug.WriteLine(e); return Task.CompletedTask; };
          api.OnHttpException += (c, e) => { Debug.WriteLine(e); return Task.CompletedTask; };
          api.WithController(() => container.GetInstance<CallbackController>());
        });
    }

    public async Task InitializeAsync()
    {
      var client = await _container.GetInstance<GitLabClientFactory>().CreateClientAsync();
      var context = _container.GetInstance<TamagitchiContext>();
      var projects = await client.Projects.GetAsync();
      var projectIdsInDb = await context.Projects.Select(x => x.GitlabId).ToArrayAsync();
      var projectsNotInDb = projects.Where(x => projectIdsInDb.Contains(x.Id) == false);
      foreach (var project in projectsNotInDb)
      {
        var entity = await context.CreateProjectAsync(project.Id, project.Name);
        await client.CreatePushWebhookAsync(_config, entity);
      }

      var users = await client.Users.GetAsync();
      var userIdsInDb = await context.Users.Select(x => x.GitlabId).ToArrayAsync();
      var usersNotInDb = users.Where(x => userIdsInDb.Contains(x.Id) == false);
      foreach (var user in usersNotInDb)
        await context.CreateUserAsync(user.Id, user.Name);

      if (File.Exists("gitlab-system.webhook") == false)
      {
        var hookClient = new SystemHookClient(client);
        var hook = await hookClient.CreateAsync(new CreateSystemHookRequest($"{_config.GetExternalUrl()}/api/callback/gitlab/repository_update")
        {
          RepositoryUpdateEvents = true,
        });

        File.WriteAllText("gitlab-system.webhook", hook.Id.ToString());
      }

      await context.SaveChangesAsync();
      _webServer.Start();
    }


    public void NotifyUserCreated(TamagitchiUser user) => OnUserCreated?.Invoke(user);
    public void NotifyProjectCreated(TamagitchiProject project) => OnProjectCreated?.Invoke(project);
    public void NotifyCommit(CommitEvent commitEvent) => OnUserCommit?.Invoke(commitEvent);

    public void Dispose()
    {
      _webServer.Dispose();
    }
  }
}
