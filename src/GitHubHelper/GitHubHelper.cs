using GitHubHelper.Model;
using GitHubHelper.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// Set version number for the assembly.
[assembly: AssemblyVersion("1.6.1.*")]

namespace GitHubHelper
{
    // See http://www.levibotelho.com/development/commit-a-file-with-the-github-api/
    public class GitHubHelper
    {
        private readonly HttpClient _client;

        public GitHubHelper()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "GalaSoft.GitHubHelper");
        }

        public GitHubHelper(HttpClient client)
        {
            _client = client;
        }

        private void BuildIssuesSection(
            StringBuilder builder,
            string projectName,
            IList<string> forMilestones,
            string issuesTitle,
            string issuesSectionTitleTemplate,
            IEnumerable<IssueInfo> issues,
            IEnumerable<IGrouping<string, Milestone>> milestones,
            bool singlePage)
        {
            builder
                .AppendLine(issuesTitle)
                .AppendLine();

            foreach (var milestonesGroup in milestones)
            {
                var relevantIssues = issues
                    .Where(i => i.Milestone.Title == milestonesGroup.Key)
                    .OrderBy(i => i.Number)
                    .ToList();

                if (forMilestones != null
                    && !forMilestones.Contains(milestonesGroup.Key)
                    || relevantIssues.Count == 0)
                {
                    continue;
                }

                var url = milestonesGroup.First().Url;

                if (string.IsNullOrEmpty(url))
                {
                    builder
                        .Append(string.Format(
                            issuesSectionTitleTemplate,
                            singlePage ? "#" : string.Empty,
                            milestonesGroup.Key,
                            url));
                }
                else
                {
                    builder
                        .Append(string.Format(
                            issuesSectionTitleTemplate,
                            singlePage ? "#" : string.Empty,
                            milestonesGroup.Key,
                            url));
                }

                if (milestonesGroup.First().ClosedLocal > DateTime.MinValue)
                {
                    builder
                        .AppendLine(
                            string.Format(
                                GitHubConstants.ClosedOn,
                                milestonesGroup.First().ClosedLocal))
                        .AppendLine();
                }
                else
                {
                    builder
                        .AppendLine(GitHubConstants.Open)
                        .AppendLine();
                }

                foreach (var issue in relevantIssues)
                {
                    string labels;

                    if (issue.Labels.Count == 0)
                    {
                        labels = "Issue";
                    }
                    else
                    {
                        labels = string.Concat(
                            issue.Labels
                                .Where(l => l.Name != projectName)
                                .Select(l => l.Name + ", "));

                        labels = labels.Substring(0, labels.Length - 2);
                    }

                    var status = GitHubConstants.Open;

                    if (issue.ClosedLocal > DateTime.MinValue)
                    {
                        status = string.Format(
                            GitHubConstants.ClosedOn,
                            milestonesGroup.First().ClosedLocal);
                    }

                    builder
                        .AppendLine(
                            string.Format(
                                GitHubConstants.IssueTemplate,
                                labels,
                                issue.Number,
                                issue.Url,
                                status,
                                issue.Title))
                        .AppendLine();
                }
            }
        }

        private string CreateReleaseNotesPageFor(
            string accountName,
            string repoName,
            string projectName,
            int projectId,
            IList<IssueInfo> issuesForPage,
            IList<string> forMilestones,
            IList<string> header,
            bool singlePage)
        {
            var builder = new StringBuilder()
                .AppendLine(
                    string.Format(
                        GitHubConstants.ReleaseNoteTitleTemplate,
                        singlePage ? "#" : string.Empty,
                        projectName,
                        string.Format(
                            GitHubConstants.ProjectUrlTemplate,
                            accountName,
                            repoName,
                            projectId)))
                .AppendLine();

            if (header != null)
            {
                foreach (var line in header)
                {
                    builder
                        .AppendLine(line)
                        .AppendLine();
                }
            }

            if (issuesForPage.Count == 0)
            {
                builder.AppendLine("> Nothing found");
            }
            else
            {
                var openComparer = new MilestoneComparer(false);
                var closeComparer = new MilestoneComparer(true);

                foreach (var issue in issuesForPage.Where(i => i.Milestone == null))
                {
                    issue.Milestone = new Milestone
                    {
                        Title = "No milestone set"
                    };

                    if (issue.ClosedLocal > DateTime.MinValue)
                    {
                        issue.Milestone.Url = string.Format(
                            GitHubConstants.GitHubClosedIssuesUrl,
                            accountName,
                            repoName);
                    }
                    else
                    {
                        issue.Milestone.Url = string.Format(
                            GitHubConstants.GitHubOpenIssuesUrl,
                            accountName,
                            repoName);
                    }
                }

                var openIssues = issuesForPage
                    .Where(i => i.State == IssueState.Open)
                    .ToList();

                if (openIssues.Count > 0)
                {
                    var milestones = openIssues
                        .Select(i => i.Milestone)
                        .OrderBy(m => m, openComparer)
                        .GroupBy(m => m.Title);

                    BuildIssuesSection(
                        builder,
                        projectName,
                        forMilestones,
                        string.Format(
                            GitHubConstants.OpenIssuesTitle,
                            singlePage ? "#" : string.Empty),
                        GitHubConstants.OpenIssuesSectionTitleTemplate,
                        openIssues,
                        milestones,
                        singlePage);
                }

                var closedIssues = issuesForPage
                    .Where(i => i.State == IssueState.Closed)
                    .ToList();

                if (closedIssues.Count > 0)
                {
                    var milestones = closedIssues
                        .Select(i => i.Milestone)
                        .OrderByDescending(m => m, closeComparer)
                        .GroupBy(m => m.Title);

                    BuildIssuesSection(
                        builder,
                        projectName,
                        forMilestones,
                        string.Format(
                            GitHubConstants.ClosedIssuesTitle,
                            singlePage ? "#" : string.Empty),
                        GitHubConstants.ClosedIssuesSectionTitleTemplate,
                        closedIssues,
                        milestones,
                        singlePage);
                }
            }

            return builder.ToString();
        }

        private string CreateReleaseNotesPageForMain(
            string accountName,
            string repoName,
            string projectName,
            IEnumerable<ReleaseNotesPageInfo> projectPages,
            IList<string> header,
            bool singlePage)
        {
            var builder = new StringBuilder()
                .AppendLine(
                    string.Format(
                        GitHubConstants.ReleaseNoteTitleTemplate,
                        string.Empty,
                        projectName,
                        string.Format(
                            GitHubConstants.RepoUrlTemplate,
                            accountName,
                            repoName)))
                .AppendLine();

            if (header != null)
            {
                foreach (var line in header)
                {
                    builder
                        .AppendLine(line)
                        .AppendLine();
                }
            }

            if (singlePage)
            {
                foreach (var page in projectPages)
                {
                    builder
                        .AppendLine(page.Markdown)
                        .AppendLine();
                }
            }
            else
            {
                builder
                    .AppendLine("## Projects in this repo")
                    .AppendLine();

                foreach (var page in projectPages)
                {
                    builder
                        .AppendLine($"- [{page.Project}]({page.Url})");
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public async Task<GetHeadResult> CommitFiles(
            string accountName,
            string repoName,
            string branchName,
            string githubToken,
            string commitMessage,
            IList<(string path, string content)> fileNamesAndContent,
            GetHeadResult existingBranchInfo = null)
        {
            if (existingBranchInfo == null)
            {
                existingBranchInfo = await GetHead(
                    accountName,
                    repoName,
                    branchName,
                    githubToken);

                if (!string.IsNullOrEmpty(existingBranchInfo.ErrorMessage))
                {
                    return new GetHeadResult
                    {
                        ErrorMessage = existingBranchInfo.ErrorMessage
                    };
                }
            }

            var mainCommit = await GetMainCommit(existingBranchInfo, githubToken);

            if (!string.IsNullOrEmpty(mainCommit.ErrorMessage))
            {
                return new GetHeadResult
                {
                    ErrorMessage = mainCommit.ErrorMessage
                };
            }

            // Post new file(s) to GitHub blob

            var treeInfos = new List<TreeInfo>();
            string jsonRequest;

            foreach (var (path, content) in fileNamesAndContent)
            {
                var uploadInfo = new UploadInfo
                {
                    Content = content
                };

                jsonRequest = JsonConvert.SerializeObject(uploadInfo);

                var uploadBlobUrl = string.Format(
                    GitHubConstants.GitHubApiBaseUrlMask,
                    accountName,
                    repoName,
                    GitHubConstants.UploadBlobUrl);

                var uploadBlobRequest = new HttpRequestMessage
                {
                    RequestUri = new Uri(uploadBlobUrl),
                    Method = HttpMethod.Post,
                    Content = new StringContent(jsonRequest)
                };

                uploadBlobRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
                uploadBlobRequest.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

                var uploadBlobResponse = await _client.SendAsync(uploadBlobRequest);

                if (uploadBlobResponse.StatusCode != HttpStatusCode.Created)
                {
                    try
                    {
                        var errorMessage = $"Error uploading blob: {await uploadBlobResponse.Content.ReadAsStringAsync()}";
                        return new GetHeadResult
                        {
                            ErrorMessage = errorMessage
                        };
                    }
                    catch (Exception ex)
                    {
                        var errorMessage = $"Unknown error uploading blob: {ex.Message}";
                        return new GetHeadResult
                        {
                            ErrorMessage = errorMessage
                        };
                    }
                }

                var uploadBlobJsonResult = await uploadBlobResponse.Content.ReadAsStringAsync();
                var uploadBlobResult = JsonConvert.DeserializeObject<ShaInfo>(uploadBlobJsonResult);

                var info = new TreeInfo(path, uploadBlobResult.Sha);
                treeInfos.Add(info);
            }

            // Create the tree

            var newTreeInfo = new CreateTreeInfo()
            {
                BaseTree = mainCommit.Tree.Sha,
            };

            newTreeInfo.AddTreeInfos(treeInfos);

            jsonRequest = JsonConvert.SerializeObject(newTreeInfo);

            var url = string.Format(
                GitHubConstants.GitHubApiBaseUrlMask,
                accountName,
                repoName,
                GitHubConstants.CreateTreeUrl);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonRequest)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

            var response = await _client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                try
                {
                    var message = $"Error creating tree: {await response.Content.ReadAsStringAsync()}";
                    return new GetHeadResult
                    {
                        ErrorMessage = message
                    };
                }
                catch (Exception ex)
                {
                    var message = $"Unknown error creating tree: {ex.Message}";
                    return new GetHeadResult
                    {
                        ErrorMessage = message
                    };
                }
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var createTreeResult = JsonConvert.DeserializeObject<ShaInfo>(jsonResult);

            // Create the commit

            var commitInfo = new CommitInfo(
                commitMessage,
                mainCommit.Sha,
                createTreeResult.Sha);

            jsonRequest = JsonConvert.SerializeObject(commitInfo);

            url = string.Format(
                GitHubConstants.GitHubApiBaseUrlMask,
                accountName,
                repoName,
                GitHubConstants.CommitUrl);

            request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonRequest)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

            response = await _client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                try
                {
                    var message = $"Error creating commit: {await response.Content.ReadAsStringAsync()}";
                    return new GetHeadResult
                    {
                        ErrorMessage = message
                    };
                }
                catch (Exception ex)
                {
                    var message = $"Unknown error creating commit: {ex.Message}";
                    return new GetHeadResult
                    {
                        ErrorMessage = message
                    };
                }
            }

            jsonResult = await response.Content.ReadAsStringAsync();
            var createCommitResult = JsonConvert.DeserializeObject<ShaInfo>(jsonResult);

            // Update reference
            var updateReferenceInfo = new UpdateReferenceInfo(createCommitResult.Sha);

            jsonRequest = JsonConvert.SerializeObject(updateReferenceInfo);

            url = string.Format(
                GitHubConstants.GitHubApiBaseUrlMask,
                accountName,
                repoName,
                string.Format(GitHubConstants.UpdateReferenceUrl, branchName));

            request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Patch,
                Content = new StringContent(jsonRequest)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

            response = await _client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                try
                {
                    var message = $"Error updating reference: {await response.Content.ReadAsStringAsync()}";
                    return new GetHeadResult
                    {
                        ErrorMessage = message
                    };
                }
                catch (Exception ex)
                {
                    var message = $"Unknown error updating reference: {ex.Message}";
                    return new GetHeadResult
                    {
                        ErrorMessage = message
                    };
                }
            }

            jsonResult = await response.Content.ReadAsStringAsync();
            var headResult = JsonConvert.DeserializeObject<GetHeadResult>(jsonResult);
            return headResult;
        }

        public async Task<GetHeadResult> CreateNewBranch(
                    string accountName,
                    string repoName,
                    string token,
                    GetHeadResult mainHead,
                    string newBranchName = null)
        {
            var newBranchRequestBody = new NewBranchInfo
            {
                Sha = mainHead.Object.Sha,
                Ref = string.Format(NewBranchInfo.RefMask, newBranchName)
            };

            var jsonRequest = JsonConvert.SerializeObject(newBranchRequestBody);

            var url = string.Format(
                GitHubConstants.GitHubApiBaseUrlMask,
                accountName,
                repoName,
                GitHubConstants.CreateNewBranchUrl);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonRequest)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

            var response = await _client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                var errorResultJson = await response.Content.ReadAsStringAsync();
                var errorResult = JsonConvert.DeserializeObject<ErrorResult>(errorResultJson);
                return new GetHeadResult
                {
                    ErrorMessage = $"Error when creating new branch: {newBranchName} / {errorResult.ErrorMessage}"
                };
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var createNewBranchResult = JsonConvert.DeserializeObject<GetHeadResult>(jsonResult);
            return createNewBranchResult;
        }

        /// <summary>
        /// At the moment, this method creates release notes with the name "release-note-label.md"
        /// at the root of the repository only. "label" corresponds to the
        /// labels passed in the createFor parameter, but URL encoded.
        /// </summary>
        /// <param name="accountName">The name of the GitHub account.</param>
        /// <param name="repoName">The name of the GitHub repo to create the release page for.</param>
        /// <param name="branchName">The name of the branch in which to commit the result.</param>
        /// <param name="createFor">A list of <see cref="ReleaseNotesPageInfo"/> with all the information for the pages to create.</param>
        /// <param name="forMilestones">A list of the milestones to create the release notes for, or null for all milestones.</param>
        /// <param name="singlePage">If true, a single release note page will be created.</param>
        /// <param name="token">The GitHub token to use for authentication.</param>
        public async Task<ReleaseNotesResult> CreateReleaseNotesMarkdown(
            string accountName,
            string repoName,
            string branchName,
            IList<ReleaseNotesPageInfo> createFor,
            IList<string> forMilestones,
            bool singlePage,
            string token)
        {
            var result = new ReleaseNotesResult();
            var issuesResult = await GetIssues(accountName, repoName, token);

            if (!string.IsNullOrEmpty(issuesResult.ErrorMessage))
            {
                result.ErrorMessage = issuesResult.ErrorMessage;
                return result;
            }

            foreach (var issue in issuesResult.Issues)
            {
                issue.Projects = new List<string>();
                var labelsToRemove = new List<IssueLabel>();

                foreach (var label in issue.Labels)
                {
                    if (createFor.Any(p => p.Project == label.Name))
                    {
                        labelsToRemove.Add(label);
                        issue.Projects.Add(label.Name);
                    }
                }

                foreach (var label in labelsToRemove)
                {
                    issue.Labels.Remove(label);
                }
            }

            var subPages = new List<ReleaseNotesPageInfo>();
            result.CreatedPages = new List<ReleaseNotesPageInfo>();

            foreach (var page in createFor.Where(p => !p.IsMainPage))
            {
                page.FilePath = string.Format(
                    GitHubConstants.ReleaseNotePageNameTemplate,
                    "-",
                    page.Project.MakeSafeName());

                page.Url = string.Format(
                    GitHubConstants.ReleaseNoteUriTemplate,
                    accountName,
                    repoName,
                    branchName,
                    page.FilePath);

                var issuesForPage = issuesResult.Issues
                    .Where(i => i.Projects.Contains(page.Project))
                    .ToList();

                page.Markdown = CreateReleaseNotesPageFor(
                    accountName,
                    repoName,
                    page.Project,
                    page.ProjectId,
                    issuesForPage,
                    forMilestones,
                    page.Header,
                    singlePage);

                subPages.Add(page);

                if (!singlePage)
                {
                    result.CreatedPages.Add(page);
                }
            }

            var mainPage = createFor.FirstOrDefault(p => p.IsMainPage);

            if (mainPage != null)
            {
                result.CreatedPages.Add(mainPage);

                mainPage.FilePath = string.Format(
                    GitHubConstants.ReleaseNotePageNameTemplate,
                    string.Empty,
                    string.Empty);

                mainPage.Url = string.Format(
                    GitHubConstants.ReleaseNoteUriTemplate,
                    accountName,
                    repoName,
                    branchName,
                    mainPage.FilePath);

                mainPage.Markdown = CreateReleaseNotesPageForMain(
                    accountName,
                    repoName,
                    mainPage.Project,
                    subPages,
                    mainPage.Header,
                    singlePage);
            }

            return result;
        }

        //public async Task<DeleteTextFileResult> DeleteFileIfExists(
        //    string accountName,
        //    string repoName,
        //    string branchName,
        //    string filePathWithExtension,
        //    string githubToken,
        //    string committerName,
        //    string committerEmail,
        //    string commitMessage)
        //{
        //    var url = string.Format(GitHubConstants.GitHubApiBaseUrlMask, accountName, repoName, filePathWithExtension);

        //    var file = await GetTextFile(accountName, repoName, branchName, filePathWithExtension, githubToken);

        //    if (!string.IsNullOrEmpty(file.ErrorMessage)
        //        || string.IsNullOrEmpty(file.Sha))
        //    {
        //        return new DeleteTextFileResult
        //        {
        //            ErrorMessage = $"Not found: {filePathWithExtension} in branch {branchName}",
        //            StatusCode = HttpStatusCode.NotFound
        //        };
        //    }

        //    var deleteInfo = new DeleteFileInfo
        //    {
        //        //Owner = accountName,
        //        //Repo = repoName,
        //        //Path = filePathWithExtension,
        //        Message = commitMessage,
        //        Branch = branchName,
        //        Sha = file.Sha,
        //        //Committer = new()
        //        //{
        //        //    Name = committerName,
        //        //    Email = committerEmail
        //        //}
        //    };

        //    var json = JsonConvert.SerializeObject(deleteInfo);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var request = new HttpRequestMessage
        //    {
        //        RequestUri = new Uri(url),
        //        Method = HttpMethod.Delete,
        //        Content = content
        //    };

        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
        //    request.Headers.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));
        //    request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");

        //    var response = await _client.SendAsync(request);
        //    var responseText = await response.Content.ReadAsStringAsync();

        //    if (response.StatusCode != HttpStatusCode.OK)
        //    {
        //        return new DeleteTextFileResult
        //        {
        //            StatusCode = response.StatusCode,
        //            ErrorMessage = responseText
        //        };
        //    }

        //    return null;

        //    //try
        //    //{
        //    //    var result = JsonConvert.DeserializeObject<GetTextFileResult>(responseText);

        //    //    if (result.Type != "file")
        //    //    {
        //    //        return new GetTextFileResult
        //    //        {
        //    //            ErrorMessage = $"{filePathWithExtension} doesn't seem to be a file on GitHub"
        //    //        };
        //    //    }

        //    //    if (string.IsNullOrEmpty(result.EncodedContent))
        //    //    {
        //    //        return new GetTextFileResult
        //    //        {
        //    //            ErrorMessage = $"{filePathWithExtension} doesn't have content"
        //    //        };
        //    //    }

        //    //    var bytes = Convert.FromBase64String(result.EncodedContent);
        //    //    result.TextContent = Encoding.UTF8.GetString(bytes);
        //    //    return result;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return new GetTextFileResult
        //    //    {
        //    //        ErrorMessage = ex.Message
        //    //    };
        //    //}
        //}

        public async Task<GetHeadResult> GetHead(
            string accountName,
            string repoName,
            string branchName,
            string token)
        {
            var url = string.Format(
                GitHubConstants.GitHubApiBaseUrlMask,
                accountName,
                repoName,
                string.Format(GitHubConstants.GetHeadUrl, branchName));

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

            var response = await _client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                try
                {
                    var errorMessage = $"Error getting head for {branchName}: {await response.Content.ReadAsStringAsync()}";
                    return new GetHeadResult
                    {
                        ErrorMessage = errorMessage
                    };
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Unknown error getting head for {branchName}: {ex.Message}";
                    return new GetHeadResult
                    {
                        ErrorMessage = errorMessage
                    };
                }
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var mainHead = JsonConvert.DeserializeObject<GetHeadResult>(jsonResult);
            return mainHead;
        }

        public async Task<IssueResult> GetIssues(
            string accountName,
            string repoName,
            string token)
        {
            var haveMoreIssues = true;
            var result = new IssueResult
            {
                Issues = new List<IssueInfo>()
            };
            var pageIndex = 1;

            while (haveMoreIssues)
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(
                        string.Format(
                            GitHubConstants.GitHubApiBaseUrlMask,
                            accountName,
                            repoName,
                            string.Format(GitHubConstants.IssuesUrl, pageIndex++))),
                    Method = HttpMethod.Get
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

                var response = await _client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new IssueResult
                    {
                        ErrorMessage = responseContent
                    };
                }

                var issues = JsonConvert.DeserializeObject<IList<IssueInfo>>(responseContent);

                if (issues.Count > 0)
                {
                    foreach (var issue in issues)
                    {
                        result.Issues.Add(issue);
                    }
                }
                else
                {
                    haveMoreIssues = false;
                }

                result.JsonFiles.Add(responseContent);
            }

            return result;
        }

        public async Task<CommitResult> GetMainCommit(
            GetHeadResult branchHead,
            string githubToken)
        {
            // Grab main commit
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(branchHead.Object.Url),
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

            var response = await _client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                try
                {
                    var errorMessage = $"Error getting commit: {await response.Content.ReadAsStringAsync()}";
                    return new CommitResult
                    {
                        ErrorMessage = errorMessage
                    };
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Unknown error getting commit: {ex.Message}";
                    return new CommitResult
                    {
                        ErrorMessage = errorMessage
                    };
                }
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var masterCommitResult = JsonConvert.DeserializeObject<CommitResult>(jsonResult);
            return masterCommitResult;
        }

        public async Task<GetTextFileResult> GetTextFile(
            string accountName,
            string repoName,
            string branchName,
            string filePathWithExtension,
            string githubToken)
        {
            var getFileUrl = string.Format(GitHubConstants.GetMarkdownFileUrl, filePathWithExtension, branchName);
            var url = string.Format(GitHubConstants.GitHubApiBaseUrlMask, accountName, repoName, getFileUrl);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(GitHubConstants.AcceptHeader));

            var response = await _client.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new GetTextFileResult
                {
                    StatusCode = response.StatusCode,
                    ErrorMessage = responseText
                };
            }

            try
            {
                var result = JsonConvert.DeserializeObject<GetTextFileResult>(responseText);

                if (result.Type != "file")
                {
                    return new GetTextFileResult
                    {
                        ErrorMessage = $"{filePathWithExtension} doesn't seem to be a file on GitHub"
                    };
                }

                if (string.IsNullOrEmpty(result.EncodedContent))
                {
                    return new GetTextFileResult
                    {
                        ErrorMessage = $"{filePathWithExtension} doesn't have content"
                    };
                }

                var bytes = Convert.FromBase64String(result.EncodedContent);
                result.TextContent = Encoding.UTF8.GetString(bytes);
                return result;
            }
            catch (Exception ex)
            {
                return new GetTextFileResult
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}