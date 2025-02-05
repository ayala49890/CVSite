using GitHubIntegration.DataEntities;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIntegration
{
    public interface IGitHubService
    {
        Task<int> GetUserFollowersAsync();
        Task<IReadOnlyList<Repository>> GetUserRepositories();
        Task<IReadOnlyList<Activity>> GetUserActivities();
        Task<int> GePublicRepositories();
        Task<Portfolio> GetUserPortfolio();
        Task<IReadOnlyList<RepositoryInfo>> GetPortfolio();
        Task<List<Repository>> SearchRepositoriesInJavaScript();
        Task<SearchRepositoryResult> SearchRepositories(string repositoryName = null, string language = null, string user = null);
        Task<DateTime> GetLastPortfolioUpdateTime();
        Task<DateTime> GetLastUserPortfolioUpdateTime();
    }
}
