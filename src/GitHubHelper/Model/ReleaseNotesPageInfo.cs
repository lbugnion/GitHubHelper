using System.Collections.Generic;

namespace GitHubHelper.Model
{
    public class ReleaseNotesPageInfo
    {
        public bool IsMainPage
        {
            get;
            set;
        }

        public string Project
        {
            get;
            set;
        }

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

        public string Markdown
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public int ProjectId
        {
            get;
            set;
        }
    }
}
