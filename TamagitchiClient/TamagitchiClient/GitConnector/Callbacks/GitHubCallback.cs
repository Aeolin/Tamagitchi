using Swan.Formatters;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.Callbacks
{
  public class GitHubCallback
  {
    [JsonProperty("action")]
    public string Action { get; set; }

    [JsonProperty("repository")]
    public GitHubRepository Repository { get; set; }

    [JsonProperty("after")]
    public string After { get; set; }

    [JsonProperty("base_ref")]
    public string BaseRef { get; set; }

    [JsonProperty("before")]
    public string Before { get; set; }

    [JsonProperty("commits")]
    public GitHubCommit[] Commits { get; set; }

    [JsonProperty("compare")]
    public string CompareUrl { get; set; }

    [JsonProperty("created")]
    public bool Created { get; set; }

    [JsonProperty("deleted")]
    public bool Deleted { get; set; }

    [JsonProperty("forced")]
    public bool Forced { get; set; }

    [JsonProperty("pusher")]
    public GitHubUser Pusher { get; set; }

    [JsonProperty("ref")]
    public string GitRef { get; set; }
  }

  public class GitHubCommit
  {
    [JsonProperty("added")]
    public string[] FilesAdded { get; set; }

    [JsonProperty("modified")]
    public string[] FilesModified { get; set; }

    [JsonProperty("removed")]
    public string[] FilesRemoved { get; set; }

    [JsonProperty("author")]
    public GitHubUser Author { get; set; }

    [JsonProperty("committer")]
    public GitHubUser Committer { get; set; }

    [JsonProperty("distinct")]
    public bool Disctinct { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("timestamp")]
    public DateTime TimeStamp { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
  }

  public class GitHubUser
  {
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("email")]
    public string EMail { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("username")]
    public string UserName { get; set; }
  }

  public class GitHubRepository
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("homepage")]
    public string Homepage { get; set; }

    [JsonProperty("private")]
    public bool Private { get; set; }

    [JsonProperty("visibility")]
    public string Visibility { get; set; }

    [JsonProperty("archived")]
    public bool Archived { get; set; }
  }
}
