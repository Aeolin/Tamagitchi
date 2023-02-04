using GitLabApiClient.Internal.Paths;
using GitLabApiClient.Models.Webhooks.Requests;
using GitLabApiClient.Models.Webhooks.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.Clients.Gitlab
{
    public interface ISystemHookClient
    {
    Task<SystemHook> GetAsync(long hookId);

    Task<IList<SystemHook>> GetAsync();

    Task<SystemHook> CreateAsync(CreateSystemHookRequest request);

    Task DeleteAsync(long hookId);

    Task<SystemHook> UpdateAsync(long hookId, CreateSystemHookRequest request);
  }
}
