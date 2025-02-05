using GitHub.API.CachedServices;
using GitHubIntegration;
using GitHubIntegration.DataEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Octokit;
namespace CV_Site.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IGitHubService _gitHubService;

        public UsersController(IConfiguration configuration, IGitHubService gitHubService)
        {
            _configuration = configuration;
            _gitHubService = gitHubService;
        }

        [HttpGet("portfolio")]
        public async Task<ActionResult<Portfolio>> GetUserPortfolio()
        {
            var portfolio = await _gitHubService.GetUserPortfolio();
            return Ok(portfolio);
        }

        [HttpGet("activities")]
        public async Task<ActionResult<IReadOnlyList<Activity>>> GetUserActivities()
        {
            var activities = await _gitHubService.GetUserActivities();
            return Ok(activities);
        }

        [HttpGet("followers")]
        public async Task<ActionResult<int>> GetUserFollowersAsync()
        {
            var followers = await _gitHubService.GetUserFollowersAsync();
            return Ok(followers);
        }

    }
}
