using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIntegration.DataEntities
{
    public class Portfolio
    {
        public string UserName { get; set; }
        public List<Repository> Repositories { get; set; }
        public string Url { get; set; }
        public int Stars { get; set; }
        public DateTimeOffset? LastCommit { get; set; }
        public int PullRequests { get; set; }
        public List<string> Languages { get; set; }
    }
}
