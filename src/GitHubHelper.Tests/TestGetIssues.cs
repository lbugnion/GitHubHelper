using GitHubHelper.Model;
using GitHubHelper.Tests.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReleaseNotesMaker.Model;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GitHubHelper.Tests;

public class TestGetIssues
{
    private readonly ILogger<TestGetIssues> _logger;

    public TestGetIssues(ILogger<TestGetIssues> logger)
    {
        _logger = logger;
    }

    [Function("TestGetIssues")]
    public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "post",
            Route = "githubhelper/test-get-issues/{kind?}")]
            HttpRequestData req,
            string kind = null)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var info = JsonConvert.DeserializeObject<GitHubInfo>(requestBody);
        var token = req.Headers.GetValues(Constants.GitHubTokenHeaderKey);

        var helper = new GitHubHelper();

        IssueResult result;

        if (!string.IsNullOrEmpty(kind))
        {
            var issuesKind = kind switch
            {
                "open" => IssuesKind.OpenIssues,
                "all" => IssuesKind.AllIssues,
                _ => throw new ArgumentException($"Invalid issues kind: {kind}")
            };

            result = await helper.GetIssues(
                info.AccountName,
                info.RepoName,
                token.First(),
                issuesKind);
        }
        else
        {
            result = await helper.GetIssues(
                info.AccountName,
                info.RepoName,
                token.First());
        }

        result = await helper.GetIssues(
            info.AccountName,
            info.RepoName,
            token.First());

        HttpResponseData response;

        if (!string.IsNullOrEmpty(result.ErrorMessage))
        {
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Error");

            return response;
        }

        response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteAsJsonAsync(result);

        return response;
    }
}