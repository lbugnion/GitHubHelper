using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GitHubHelper.Tests.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReleaseNotesMaker.Model;

namespace GitHubHelper.Tests
{
    public class TestDeleteFile
    {
        private readonly ILogger _logger;

        public TestDeleteFile(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TestDeleteFile>();
        }

        [Function(nameof(TestDeleteFile))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function, 
                "post",
            Route = "githubhelper/test-delete-file")]
            HttpRequestData req)
        {
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //var info = JsonConvert.DeserializeObject<GitHubInfo>(requestBody);
            //var token = req.Headers.GetValues(Constants.GitHubTokenHeaderKey);

            //var helper = new GitHubHelper();

            //var result = await helper.DeleteFileIfExists(
            //    info.AccountName,
            //    info.RepoName,
            //    info.BranchName,
            //    info.FilePath,
            //    token.First(),
            //    info.CommitterName,
            //    info.CommitterEmail,
            //    info.CommitMessage);

            HttpResponseData response;

            //if (result.StatusCode != HttpStatusCode.OK)
            //{
            //    response = req.CreateResponse(result.StatusCode);
            //    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            //    return response;
            //}

            response = req.CreateResponse(HttpStatusCode.NoContent);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("Not implemented");

            return response;
        }
    }
}
