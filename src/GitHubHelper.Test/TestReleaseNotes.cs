using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReleaseNotesMaker.Model;

namespace GitHubHelper.Test
{
    public static class TestReleaseNotes
    {
        private const string GitHubTokenHaderKey = "x-githubhelper-test-token";

        [FunctionName(nameof(TestReleaseNotes))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous, 
                "post", 
                Route = "githubhelper/test/{milestones}")] 
            HttpRequest req,
            string milestones,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var info = JsonConvert.DeserializeObject<GitHubInfo>(requestBody);
            var token = req.Headers[GitHubTokenHaderKey];

            var forMilestones = (milestones == "all")
                ? null
                : milestones.Split(new char[]
                {
                    ','
                }).ToList();

            var helper = new GitHubHelper();

            var releaseNotes = await helper.CreateReleaseNotesMarkdown(
                info.AccountName,
                info.RepoName,
                info.BranchName,
                info.Projects,
                forMilestones,
                info.SinglePage,
                token);

            var resultBuilder = new StringBuilder();

            foreach (var page in releaseNotes.CreatedPages)
            {
                resultBuilder
                    .AppendLine($"> {page.FilePath}")
                    .AppendLine()
                    .AppendLine(page.Markdown)
                    .AppendLine();
            }

            return new OkObjectResult(resultBuilder.ToString());
        }
    }
}

