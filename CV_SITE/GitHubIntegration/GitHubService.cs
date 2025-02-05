using GitHubIntegration.DataEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace GitHubIntegration
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubIntegrationOptions _options;
        public GitHubService(IOptions<GitHubIntegrationOptions> options)
        {
            _options = options.Value;
            _client = new GitHubClient(new ProductHeaderValue("my-amazing-app"));
            var tokenAuth = new Credentials(_options.Token);
            if (tokenAuth != null)
                _client.Credentials = tokenAuth;
        }

        public async Task<int> GetUserFollowersAsync()
        {
            var user = await _client.User.Get(_options.UserName);
            return user.Followers;
        }
        public async Task<IReadOnlyList<Repository>> GetUserRepositories()
        {
            return await _client.Repository.GetAllForCurrent();
        }
        public async Task<IReadOnlyList<Activity>> GetUserActivities()
        {
            return await _client.Activity.Events.GetAllUserPerformed(_options.UserName);
        }
        public async Task<int> GePublicRepositories()
        {
            var user = await _client.User.Get(_options.UserName);
            return user.PublicRepos;
        }

        public async Task<List<Repository>> SearchRepositoriesInJavaScript()
        {
            var request = new SearchRepositoriesRequest("Library") { Language = Language.JavaScript };
            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }
        public async Task<Portfolio> GetUserPortfolio()
        {
            var portfolio = new Portfolio();
            portfolio.UserName = _options.UserName;
            portfolio.Repositories = (await _client.Repository.GetAllForCurrent()).ToList();
            return portfolio;
        }
        public async Task<IReadOnlyList<RepositoryInfo>> GetPortfolio()
        {
            var repositories = await _client.Repository.GetAllForCurrent();
            var portfolio = new List<RepositoryInfo>();

            foreach (var repo in repositories)
            {
                portfolio.Add(new RepositoryInfo
                {
                    Name = repo.Name,
                    Language = repo.Language,
                    LastCommit = (await _client.Repository.Commit.GetAll(repo.Owner.Login, repo.Name)).FirstOrDefault()?.Commit.Author.Date,
                    Stars = repo.StargazersCount,
                    PullRequests = (await _client.PullRequest.GetAllForRepository(repo.Id)).Count,
                    WebsiteUrl = repo.HtmlUrl
                });
            }

            return portfolio;
        }
        public async Task<SearchRepositoryResult> SearchRepositories(string repositoryName = null, string language = null, string user = null)
        {
            var request = new SearchRepositoriesRequest
            {
                Language = string.IsNullOrWhiteSpace(language) ? null : (Language?)Enum.Parse(typeof(Language), language, true),
                User = user,
            };
            if (!string.IsNullOrWhiteSpace(repositoryName))
            {
                request.Parameters.Add("q", repositoryName);
            }

            return await _client.Search.SearchRepo(request);


        }
        public async Task<DateTime> GetLastPortfolioUpdateTime()
        {
            var repos = await _client.Repository.GetAllForCurrent();
            return repos.Max(repo => repo.UpdatedAt.DateTime);
        }

        public async Task<DateTime> GetLastUserPortfolioUpdateTime()
        {
            var repos = await _client.Repository.GetAllForCurrent();
            return repos.Max(repo => repo.UpdatedAt.DateTime);
        }


    }



}
