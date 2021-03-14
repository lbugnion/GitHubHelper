namespace GitHubHelper.Model
{
    public static class GitHubConstants
    {
        public const string AcceptHeader = "application/vnd.github.v3+json";
        public const string ClosedIssuesSectionTitleTemplate = "{0}### Fixed issues in [{1}]({2})";
        public const string ClosedIssuesTitle = "{0}## Closed issues";
        public const string ClosedOn = " *(closed on {0:dd MMM yyyy})*";
        public const string CommitUrl = "git/commits";
        public const string CreateNewBranchUrl = "git/refs";
        public const string CreateTreeUrl = "git/trees";
        public const string GetHeadUrl = "git/ref/heads/{0}";
        public const string GetMarkdownFileUrl = "contents/{0}?ref={1}";
        public const string GitHubApiBaseUrlMask = "https://api.github.com/repos/{0}/{1}/{2}";
        public const string GitHubClosedIssuesUrl = "https://github.com/{0}/{1}/issues?q=is%3Aissue+is%3Aclosed";
        public const string GitHubDateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
        public const string GitHubOpenIssuesUrl = "https://github.com/{0}/{1}/issues";
        // TODO Implement paging for issues
        public const string IssuesUrl = "issues?state=all&per_page=100";
        public const string IssueTemplate = "[{0} # {1}]({2}){3}: {4}";
        public const string Open = " *(open)*";
        public const string OpenIssuesSectionTitleTemplate = "{0}### Planned for [{1}]({2})";
        public const string OpenIssuesTitle = "{0}## Known issues (still open)";
        public const string PlannedFor = "*(planned for {0:dd MMM yyyy})*";
        public const string ProjectUrlTemplate = "https://github.com/{0}/{1}/projects/{2}";
        public const string ReleaseNotePageNameTemplate = "release-notes{0}{1}.md";
        public const string ReleaseNoteTitleTemplate = "{0}# Release notes for [{1}]({2})";
        public const string ReleaseNoteUriTemplate = "https://github.com/{0}/{1}/blob/{2}/{3}";
        public const string RepoUrlTemplate = "https://github.com/{0}/{1}";
        public const string UpdateReferenceUrl = "git/refs/heads/{0}";
        public const string UploadBlobUrl = "git/blobs";
    }
}