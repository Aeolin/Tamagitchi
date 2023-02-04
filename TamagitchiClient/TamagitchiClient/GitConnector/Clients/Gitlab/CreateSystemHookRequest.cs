using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.Clients.Gitlab
{
  public class CreateSystemHookRequest
  {
    public CreateSystemHookRequest(string url)
    {
      this.Url = url;
    }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("push_events")]
    public bool PushEvents { get; set; }

    [JsonProperty("tag_push_events")]
    public bool TagPushEvents { get; set; }

    [JsonProperty("merge_request_events")]
    public bool MergeRequestEvents { get; set; }

    [JsonProperty("repository_update_events")]
    public bool RepositoryUpdateEvents { get; set; }

    [JsonProperty("enable_ssl_verification")]
    public bool EnableSSLVerification { get; set; }
  }
}
