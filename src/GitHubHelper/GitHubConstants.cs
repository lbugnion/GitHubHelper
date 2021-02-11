namespace GitHubHelper
{
    public static class GitHubConstants
    {
        public const string ReleaseNoteTitleTemplate = "# Release notes for [{0}]({1})";
        public const string OpenIssuesTitle = "## Known issues (still open)";
        public const string OpenIssuesSectionTitleTemplate = "### Planned for [{0}]({1})";
        public const string IssueTemplate = "[{0} # {1}]({2}){3}: {4}";
        public const string ClosedIssuesTitle = "## Closed issues";
        public const string ClosedIssuesSectionTitleTemplate = "### Fixed issues in [{0}]({1})";
        public const string ClosedOn = " *(closed on {0:dd MMM yyyy})*";
        public const string Open = " *(open)*";
        public const string PlannedFor = "*(planned for {0:dd MMM yyyy})*";
        public const string ReleaseNoteUriTemplate = "https://github.com/{0}/{1}/blob/main/{2}";
        public const string ReleaseNotePageNameTemplate = "release-notes-{0}.md";
        public const string ProjectUrlTemplate = "https://github.com/{0}/{1}/projects/{2}";
    }
}
