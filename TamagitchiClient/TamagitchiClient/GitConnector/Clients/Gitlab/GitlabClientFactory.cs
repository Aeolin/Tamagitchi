using GitLabApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GitConnector.Clients.Gitlab
{
    public class GitLabClientFactory
    {
        private GitLabClientConfig _config;

        public GitLabClientFactory(GitLabClientConfig config)
        {
            _config = config;
        }

        public async Task<GitLabClient> CreateClientAsync()
        {
            if (_config.Token != null)
            {
                return new GitLabClient(_config.Host, _config.Token);
            }
            else if (_config.Username != null && _config.Password != null)
            {
                var client = new GitLabClient(_config.Host);
                var result = await client.LoginAsync(_config.Username, _config.Password);
                return client;
            }
            else
            {
                throw new ArgumentException("config needs username and password or token");
            }
        }


    }
}
