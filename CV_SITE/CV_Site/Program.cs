using GitHub.API.CachedServices;
using GitHubIntegration;
using Octokit;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var GITHUBUSER = builder.Configuration.GetSection("GitHubIntegrationOptions")["UserName"];

// Add services to the container.

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IGitHubService, GitHubService>();   
builder.Services.Decorate<IGitHubService,CachedGitHubService>();
builder.Services.AddGitHubIntegration(options => builder.Configuration.GetSection("GitHubIntegrationOptions").Bind(options));

//builder.Services.Configure<GitHubIntegrationOptions>(builder.Configuration.GetSection(nameof(GitHubIntegrationOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
