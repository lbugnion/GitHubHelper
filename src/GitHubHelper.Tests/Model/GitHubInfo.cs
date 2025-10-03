using GitHubHelper.Model;
using System.Collections.Generic;

namespace ReleaseNotesMaker.Model
{
    public class GitHubInfo
    {
        public string AccountName
        {
            get;
            set;
        }

        public string RepoName
        {
            get;
            set;
        }

        public string BranchName
        {
            get;
            set;
        }

        public string FilePath
        {
            get;
            set;
        }

        public IList<ReleaseNotesPageInfo> Projects
        {
            get;
            set;
        }

        public string CommitMessage
        {
            get;
            set;
        }

        public bool SinglePage 
        { 
            get; 
            set; 
        }

        public string CommitterName
        {
            get;
            set;
        }

        public string CommitterEmail
        {
            get;
            set;
        }

    }
}
