using GitHubIntegration;
using GitHubIntegration.DataEntities;
using Microsoft.Extensions.Caching.Memory;
using Octokit;

namespace GitHub.API.CachedServices
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;
        private const string PortfolioKey = "PortfolioKey";
        private const string UserPortfolioKey = "UserPortfolioKey";

        private DateTime? _lastUpdatedPortfolio;
        private DateTime? _lastUpdatedUserPortfolio;

        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }

        public async Task<IReadOnlyList<RepositoryInfo>> GetPortfolio()
        {
            if (await IsCacheValid(PortfolioKey, _gitHubService.GetLastPortfolioUpdateTime()))
            {
                return _memoryCache.Get<IReadOnlyList<RepositoryInfo>>(PortfolioKey);
            }

            var portfolio = await _gitHubService.GetPortfolio();
            UpdateCache(PortfolioKey, portfolio, _gitHubService.GetLastPortfolioUpdateTime());
            return portfolio;
        }

        public async Task<Portfolio> GetUserPortfolio()
        {
            if (await IsCacheValid(UserPortfolioKey, _gitHubService.GetLastUserPortfolioUpdateTime()))
            {
                return _memoryCache.Get<Portfolio>(UserPortfolioKey);
            }

            var userPortfolio = await _gitHubService.GetUserPortfolio();
            UpdateCache(UserPortfolioKey, userPortfolio, _gitHubService.GetLastUserPortfolioUpdateTime());
            return userPortfolio;
        }

        public async Task<DateTime> GetLastPortfolioUpdateTime()
        {
            return await _gitHubService.GetLastPortfolioUpdateTime();
        }

        public async Task<DateTime> GetLastUserPortfolioUpdateTime()
        {
            return await _gitHubService.GetLastUserPortfolioUpdateTime();
        }

        private async Task<bool> IsCacheValid(string cacheKey, Task<DateTime> lastUpdatedTimeTask)
        {
            var lastUpdated = await lastUpdatedTimeTask;

            if (cacheKey == PortfolioKey && _lastUpdatedPortfolio.HasValue && _lastUpdatedPortfolio >= lastUpdated)
            {
                return _memoryCache.TryGetValue(cacheKey, out _);
            }

            if (cacheKey == UserPortfolioKey && _lastUpdatedUserPortfolio.HasValue && _lastUpdatedUserPortfolio >= lastUpdated)
            {
                return _memoryCache.TryGetValue(cacheKey, out _);
            }

            return false;
        }

        private void UpdateCache<T>(string cacheKey, T data, Task<DateTime> lastUpdatedTimeTask)
        {
            var lastUpdatedTime = lastUpdatedTimeTask.Result;

            if (cacheKey == PortfolioKey)
            {
                _lastUpdatedPortfolio = lastUpdatedTime;
            }
            else if (cacheKey == UserPortfolioKey)
            {
                _lastUpdatedUserPortfolio = lastUpdatedTime;
            }

            _memoryCache.Set(cacheKey, data, new MemoryCacheEntryOptions().
                SetAbsoluteExpiration(TimeSpan.FromMinutes(30)).
                SetSlidingExpiration(TimeSpan.FromMinutes(10)));
           
        }

        public async Task<int> GePublicRepositories() => await _gitHubService.GePublicRepositories();

        public async Task<IReadOnlyList<Activity>> GetUserActivities() => await _gitHubService.GetUserActivities();

        public async Task<int> GetUserFollowersAsync() => await _gitHubService.GetUserFollowersAsync();

        public async Task<IReadOnlyList<Repository>> GetUserRepositories() => await _gitHubService.GetUserRepositories();

        public async Task<SearchRepositoryResult> SearchRepositories(string repositoryName = null, string language = null, string user = null) =>
            await _gitHubService.SearchRepositories(repositoryName, language, user);

        public async Task<List<Repository>> SearchRepositoriesInJavaScript() => await _gitHubService.SearchRepositoriesInJavaScript();
    }
}
