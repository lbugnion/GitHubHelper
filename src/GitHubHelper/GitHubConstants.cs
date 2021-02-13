namespace GitHubHelper
{
    public static class GitHubConstants
    {
        public const string ReleaseNoteTitleTemplate = "{0}# Release notes for [{1}]({2})";
        public const string OpenIssuesTitle = "{0}## Known issues (still open)";
        public const string OpenIssuesSectionTitleTemplate = "{0}### Planned for [{1}]({2})";
        public const string IssueTemplate = "[{0} # {1}]({2}){3}: {4}";
        public const string ClosedIssuesTitle = "{0}## Closed issues";
        public const string ClosedIssuesSectionTitleTemplate = "{0}### Fixed issues in [{1}]({2})";
        public const string ClosedOn = " *(closed on {0:dd MMM yyyy})*";
        public const string Open = " *(open)*";
        public const string PlannedFor = "*(planned for {0:dd MMM yyyy})*";
        public const string ReleaseNoteUriTemplate = "https://github.com/{0}/{1}/blob/{2}/{3}";
        public const string ReleaseNotePageNameTemplate = "release-notes{0}{1}.md";
        public const string ProjectUrlTemplate = "https://github.com/{0}/{1}/projects/{2}";
        public const string RepoUrlTemplate = "https://github.com/{0}/{1}";
    }
}
