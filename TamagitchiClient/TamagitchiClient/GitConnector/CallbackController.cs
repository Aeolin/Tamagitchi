using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using GitLabApiClient;
using GitLabApiClient.Internal.Paths;
using GitLabApiClient.Models.Projects.Responses;
using GitLabApiClient.Models.Webhooks.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swan.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TamagitchiClient.Database;
using TamagitchiClient.Database.Models;
using TamagitchiClient.GitConnector.Callbacks;
using TamagitchiClient.GitConnector.Clients.Gitlab;
using TamagitchiClient.GitConnector.Models;

namespace TamagitchiClient.GitConnector
{
  public class CallbackController : WebApiController
  {
    private IConfiguration _config;
    private GitLabClient _gitlabClient;
    private GitLabClientFactory _clientFactory;
    private TamagitchiContext _tamagitchiContext;
    private GitConnector _connector;

    protected override void OnBeforeHandler()
    {
      base.OnBeforeHandler();
    }

    public CallbackController(GitLabClientFactory factory, TamagitchiContext tamagitchiContext, IConfiguration config, GitConnector connector)
    {
      _tamagitchiContext=tamagitchiContext;
      _clientFactory = factory;
      _config = config;
      _connector=connector;
    }

    private async Task<GitLabClient> getClientAsync()
    {
      if (_gitlabClient== null)
        _gitlabClient = await _clientFactory.CreateClientAsync();

      return _gitlabClient;
    }

    [Route(HttpVerbs.Post, "/callback/gitlab/repository_update")]
    public async Task ReceiveGitlabWebhook_RepositoryUpdate()
    {
      var body = await HttpContext.GetRequestBodyAsStringAsync();
      var callback = Json.Deserialize<GitlabSystemHookCallback>(body, JsonSerializerCase.None);
      switch (callback.EventName)
      {
        case "project_create":
          handle_ReceiveGitlabWebhook_ProjectCreated(Json.Deserialize<GitLabProjectCreatedCallback>(body, JsonSerializerCase.None));
          break;

        case "user_create":
          handle_ReceiveGitlabWebhook_UserCreated(Json.Deserialize<GitLabUserCreatedCallback>(body, JsonSerializerCase.None));
          break;
      }
    }


    [Route(HttpVerbs.Post, "/callback/gitlab/push/{id}")]
    public async Task ReceiveGitlabWebhook_Push(Guid id, [JsonData] GitLabPushCallback callback)
    {
      var client = await getClientAsync();
      var project = await _tamagitchiContext.Projects.FindAsync(id);
      var user = await _tamagitchiContext.Users.FirstOrDefaultAsync(x => x.GitlabId == callback.UserId);

      foreach (var commit in callback.Commits)
      {
        var diffs = await client.Commits.GetDiffsAsync(callback.ProjectId, commit.Id);
        var differences = diffs.SelectMany(x => DiffParser.Parser.GetDiffs(x.DiffText, x.NewPath, x.OldPath)).ToArray();
        _connector.NotifyCommit(new CommitEvent(project, user, differences));
      }
    }

    protected async void handle_ReceiveGitlabWebhook_ProjectCreated([JsonData] GitLabProjectCreatedCallback callback)
    {
      var client = await getClientAsync();
      var project = await _tamagitchiContext.CreateProjectAsync(callback.ProjectId, callback.Name);
      await client.CreatePushWebhookAsync(_config, project);
      await _tamagitchiContext.SaveChangesAsync();
      _connector.NotifyProjectCreated(project);
    }

    protected async void handle_ReceiveGitlabWebhook_UserCreated([JsonData] GitLabUserCreatedCallback callback)
    {
      var user = new TamagitchiUser { GitlabId = callback.UserId, Name = callback.Name };
      await _tamagitchiContext.Users.AddAsync(user);
      await _tamagitchiContext.SaveChangesAsync();
      _connector.NotifyUserCreated(user);
    }
  }
}
