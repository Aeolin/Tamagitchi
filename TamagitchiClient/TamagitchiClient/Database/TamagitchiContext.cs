using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Database.Models;
using TamagitchiClient.TamagotchiLogic.Models;

namespace TamagitchiClient.Database
{
  public class TamagitchiContext : DbContext
  {
    public TamagitchiContext() { }

    public TamagitchiContext(DbContextOptions<TamagitchiContext> options) : base(options)
    {

    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (optionsBuilder.IsConfigured == false)
        optionsBuilder.UseSqlServer();

      optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<TamagitchiPet>()
        .HasMany(x => x.TamagitchiCharacterTraits)
        .WithOne(x => x.Pet);

      builder.Entity<TamagitchiCharacterTrait>()
        .Property(x => x.Trait)
          .HasConversion(x => (int)x, x => (CharacterTrait)x);

      foreach (var entity in builder.Model.GetEntityTypes())
        entity.SetTableName($"Tamagitchi.{entity.GetTableName()}");

    }

    public async Task<TamagitchiUser> CreateUserAsync(long gitlabId, string name)
    {
      var user = await Users.FirstOrDefaultAsync(x => x.GitlabId == gitlabId);
      if (user == null)
      {
        user = new TamagitchiUser { GitlabId= gitlabId, Name = name };
        await Users.AddAsync(user);
        var pet = new TamagitchiPet
        {
          Name = "Yellymew",
          Alive = true,
          CurrentHealth = 50,
          MaxHealth = 50,
          Owner = user,
          TamagitchiCharacterTraits = new List<TamagitchiCharacterTrait>()
        {
          new TamagitchiCharacterTrait(CharacterTrait.Cheerfull),
          new TamagitchiCharacterTrait(CharacterTrait.Friendly),
          new TamagitchiCharacterTrait(CharacterTrait.Happy),
          new TamagitchiCharacterTrait(CharacterTrait.Loving),
          new TamagitchiCharacterTrait(CharacterTrait.Energetic)
        },
          LastFood = DateTime.UtcNow
        };
        await Pets.AddAsync(pet);
        await SaveChangesAsync();
      }
      
      return user;
    }

    public async Task<TamagitchiProject> CreateProjectAsync(long id, string name)
    {
      var project = await Projects.FirstOrDefaultAsync(x => x.GitlabId== id);
      if (project == null)
      {
        project = new TamagitchiProject { GitlabId = id, Name = name };
        await Projects.AddAsync(project);
        await SaveChangesAsync();
      }
      return project;
    }

    public DbSet<TamagitchiUser> Users { get; set; }
    public DbSet<TamagitchiPet> Pets { get; set; }
    public DbSet<TamagitchiCharacterTrait> CharacterTraits { get; set; }
    public DbSet<TamagitchiProject> Projects { get; set; }
  }
}
