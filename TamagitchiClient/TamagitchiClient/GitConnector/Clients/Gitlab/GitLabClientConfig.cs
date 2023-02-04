using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.Clients.Gitlab
{
  public class GitLabClientConfig
  {
    public string Host { get; set; }
    public string Token { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }
}
