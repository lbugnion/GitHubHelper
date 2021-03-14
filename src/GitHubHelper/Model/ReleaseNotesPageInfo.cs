using System.Collections.Generic;

namespace GitHubHelper.Model
{
    public class ReleaseNotesPageInfo
    {
        public string FilePath
        {
            get;
            set;
        }

        public IList<string> Header
        {
            get;
            set;
        }

        public bool IsMainPage
        {
            get;
            set;
        }

        public string Markdown
        {
            get;
            set;
        }

        public string Project
        {
            get;
            set;
        }

        public int ProjectId
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }
    }
}