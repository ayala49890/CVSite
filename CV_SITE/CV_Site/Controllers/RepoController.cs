using GitHubIntegration;
using GitHubIntegration.DataEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Octokit;
namespace CV_Site.Controllers
{
    [Route("api/[controller]")]
    public class RepositoryController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public RepositoryController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        // Get Portfolio
        [HttpGet("portfolio")]
        public async Task<ActionResult<IReadOnlyList<RepositoryInfo>>> GetPortfolio()
        {
            var portfolio = await _gitHubService.GetPortfolio();
            return Ok(portfolio);
        }

        // Get User Repositories
        [HttpGet("user-repositories")]
        public async Task<ActionResult<IReadOnlyList<Repository>>> GetUserRepositories()
        {
            var repositories = await _gitHubService.GetUserRepositories();
            return Ok(repositories);
        }

        // Search Repositories by Name, Language, User
        [HttpGet("search")]
        public async Task<ActionResult<SearchRepositoryResult>> SearchRepositories([FromQuery] string repositoryName = null, [FromQuery] string language = null, [FromQuery] string user = null)
        {
            var result = await _gitHubService.SearchRepositories(repositoryName, language, user);
            return Ok(result);
        }

        // Search Repositories in JavaScript
        [HttpGet("search/javascript")]
        public async Task<ActionResult<List<Repository>>> SearchRepositoriesInJavaScript()
        {
            var repositories = await _gitHubService.SearchRepositoriesInJavaScript();
            return Ok(repositories);
        }
    }
}
