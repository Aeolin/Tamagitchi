using Swan.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.Callbacks
{
  public class GitLabPushCallback
  {
    [JsonProperty("object_kind")]
    public string ObjectKind { get; set; }

    [JsonProperty("event_name")]
    public string EventName { get; set; }

    [JsonProperty("before")]
    public string Before { get; set; }

    [JsonProperty("after")]
    public string After { get; set; }

    [JsonProperty("ref")]
    public string GitRef { get; set; }

    [JsonProperty("checkout_sha")]
    public string CheckoutSha { get; set; }

    [JsonProperty("user_id")]
    public long UserId { get; set; }

    [JsonProperty("user_name")]
    public string UserName { get; set; }

    [JsonProperty("user_username")]
    public string UserUsername { get; set; }

    [JsonProperty("user_email")]
    public string UserEmail { get; set; }

    [JsonProperty("user_avatar")]
    public string UserAvatar { get; set; }

    [JsonProperty("project_id")]
    public int ProjectId { get; set; }

    [JsonProperty("project")]
    public GitLabProject Project { get; set; }

    [JsonProperty("repository")]
    public GitLabRepository Repository { get; set; }

    [JsonProperty("commits")]
    public GitLabCommit[] Commits { get; set; }
  }

  public class GitLabCommit
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("author")]
    public GitLabAuthor Author { get; set; }

    [JsonProperty("added")]
    public string[] FilesAdded { get; set; }

    [JsonProperty("modified")]
    public string[] FilesModified { get; set; }

    [JsonProperty("removed")]
    public string[] FilesRemoved { get; set; }
  }

  public class GitLabAuthor
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
  }

  public class GitLabRepository
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("homepage")]
    public string Homepage { get; set; }

    [JsonProperty("git_http_url")]
    public string GitHttpUrl { get; set; }

    [JsonProperty("git_ssh_url")]
    public string GitSshUrl { get; set; }

    [JsonProperty("visibility_level")]
    public int Visibility { get; set; }
  }

  public class GitlabSystemHookCallback
  {
    [JsonProperty("event_name")]
    public string EventName { get; set; }
  }

  public class GitLabProject
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("web_url")]
    public string WebUrl { get; set; }

    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonProperty("git_ssh_url")]
    public string GitSshUrl { get; set; }

    [JsonProperty("git_http_url")]
    public string GitHttpUrl { get; set; }

    [JsonProperty("namespace")]
    public string Namespace { get; set; }

    [JsonProperty("visibility_level")]
    public int VisibilityLevel { get; set; }

    [JsonProperty("path_with_namespace")]
    public string PathWithNamespace { get; set; }

    [JsonProperty("default_branch")]
    public string DefaultBranch { get; set; }

    [JsonProperty("homepage")]
    public string Homepage { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("ssh_url")]
    public string SshUrl { get; set; }

    [JsonProperty("http_url")]
    public string HttpUrl { get; set; }
  }

  public class GitLabProjectCreatedCallback
  {
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("event_name")]
    public string EventName { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("owner_name")]
    public string OwnerName { get; set; }

    [JsonProperty("owner_email")]
    public string OwnerEmail { get; set; }

    [JsonProperty("path_with_namespace")]
    public string PathWithNamespace { get; set; }
    public string Namespace => PathWithNamespace.Split('/').First();

    [JsonProperty("project_id")]
    public int ProjectId { get; set; }

    [JsonProperty("project_visibility")]
    public string ProjectVisibility { get; set; } 
  }

  public class GitLabUserCreatedCallback
  {
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("event_name")]
    public string EventName { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("user_id")]
    public int UserId { get; set; }
  }
}
