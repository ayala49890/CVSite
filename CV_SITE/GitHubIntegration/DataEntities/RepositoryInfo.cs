using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIntegration.DataEntities
{
    public class RepositoryInfo
    {
            public string Name { get; set; }
            public string Language { get; set; }
            public DateTimeOffset? LastCommit { get; set; }
            public int Stars { get; set; }
            public int PullRequests { get; set; }
            public string WebsiteUrl { get; set; }
        
    }
}
