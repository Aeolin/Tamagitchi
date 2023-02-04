using GitLabApiClient;
using GitLabApiClient.Internal.Paths;
using GitLabApiClient.Models.Projects.Responses;
using GitLabApiClient.Models.Webhooks.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.Clients.Gitlab
{
  public class SystemHookClient : ISystemHookClient
  {
    protected delegate Task<T> PostDelegate<T>(string uri, object data = null);
    protected delegate Task<T> PutDelegate<T>(string uri, object data = null);
    protected delegate Task<IList<T>> GetPagedListDelegate<T>(string uri);
    protected delegate Task<T> GetDelegate<T>(string uri);
    protected delegate Task DeleteDelegate(string uri);

    protected readonly PostDelegate<SystemHook> _post;
    protected readonly PutDelegate<SystemHook> _put;
    protected readonly GetPagedListDelegate<SystemHook> _getPagedList;
    protected readonly GetDelegate<SystemHook> _get;
    protected readonly DeleteDelegate _delete;

    public SystemHookClient(GitLabClient gitlabClient)
    {
      // Black magic fuckery cuz gitlab api library i use, used "proper" oop and doesnt allow access to internal classes => me can't extend normally
      var sysHookType = typeof(SystemHook);
      var http = typeof(GitLabClient).GetField("_httpFacade", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gitlabClient);
      var type = http.GetType();
      _post = type.GetMethod("Post", 1, new[] { typeof(string), typeof(object) })
        .MakeGenericMethod(sysHookType)
        .CreateDelegate<PostDelegate<SystemHook>>(http);

      _put = type.GetMethod("Put", 1, new[] { typeof(string), typeof(object) })
        .MakeGenericMethod(sysHookType)
        .CreateDelegate<PutDelegate<SystemHook>>(http);

      _getPagedList = type.GetMethod("GetPagedList")
        .MakeGenericMethod(sysHookType)
        .CreateDelegate<GetPagedListDelegate<SystemHook>>(http);

      _get = type.GetMethod("Get")
        .MakeGenericMethod(sysHookType)
        .CreateDelegate<GetDelegate<SystemHook>>(http);

      _delete = type.GetMethod("Delete", new[] { typeof(string) })
        .CreateDelegate<DeleteDelegate>(http);
    }

    public async Task<SystemHook> CreateSystemHookAsync(CreateSystemHookRequest request)
    {
      return await _post("hooks", request);
    }

    public async Task<SystemHook> GetAsync(long hookId)
    {
      return await _get($"hooks/{hookId}");
    }

    public async Task<IList<SystemHook>> GetAsync()
    {
      return await _getPagedList("hooks");
    }

    public async Task<SystemHook> CreateAsync(CreateSystemHookRequest request)
    {
      return await _post("hooks", request);
    }

    public async Task DeleteAsync(long hookId)
    {
      await _delete($"hooks/{hookId}");
    }

    public async Task<SystemHook> UpdateAsync(long hookId, CreateSystemHookRequest request)
    {
      return await _put($"hooks/{hookId}", request);
    }
  }
}
