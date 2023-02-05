using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;
using ReInject;
using ReInject.Interfaces;
using ReInject.PostInjectors.EventInjection;
using System.Collections.Generic;
using TamagitchiClient;
using TamagitchiClient.Database;
using TamagitchiClient.GitConnector;
using TamagitchiClient.GitConnector.Clients.Gitlab;
using TamagitchiClient.GPT3Prompts;
using TamagitchiClient.TamagotchiLogic;
using TamagitchiClient.TamagotchiLogic.Models;
using System;
using reInject.PostInjectors.BackgroundWorker;
using Microsoft.Extensions.Logging;
using Swan.Logging;

var config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
var factory = LoggerFactory.Create(logger => {
  logger.AddDebug();
  logger.AddConfiguration(config.GetSection("Logging"));
});

var container = Injector.GetContainer();
container.Register<ILoggerFactory>(DependencyStrategy.AtomicInstance, true, factory);
container.Register<IConfiguration>(DependencyStrategy.AtomicInstance, true, config);
container.Register<IDependencyContainer>(DependencyStrategy.AtomicInstance, true, container);
var gitlabConfig = config.GetSection("GitlabApi").Get<GitLabClientConfig>();
container.Register<GitLabClientConfig>(DependencyStrategy.AtomicInstance, true, gitlabConfig);
container.Register<GitLabClientFactory>(DependencyStrategy.SingleInstance, true);
var dbBuilder = new DbContextOptionsBuilder<TamagitchiContext>().UseSqlServer(config.GetConnectionString("TamagitchiDatabase"));
container.Register<DbContextOptions<TamagitchiContext>>(DependencyStrategy.AtomicInstance, true, dbBuilder.Options);
container.Register<TamagitchiContext>(DependencyStrategy.NewInstance, true);
using (var ctx = container.GetInstance<TamagitchiContext>())
  await ctx.Database.MigrateAsync();

container.AddOpenAIServices(x =>
{
  x.ApiKey = config.GetValue<string>("OpenApi:ApiKey");
});

container.Register<IPromptGenerator, PromptGenerator>(DependencyStrategy.CachedInstance, true);
var connector = container.GetInstance<GitConnector>();
await connector.InitializeAsync();
container.AddEventInjector(x =>
{
  x.RegisterEventSources(connector, "GitConnector:");
});
container.AddBackgroundWorker(x => { x.SchedulePeriod = TimeSpan.FromMinutes(5); }, "BackgroundWorker", factory);
container.Register<GitConnector>(DependencyStrategy.AtomicInstance, true, connector);
var coreLogic = container.GetInstance<TamagotchiCore>();
container.Register<TamagotchiCore>(DependencyStrategy.AtomicInstance, true, coreLogic);
using var game = container.GetInstance<Tamagitchi>();
game.Run();
connector.Dispose();
coreLogic.Dispose();