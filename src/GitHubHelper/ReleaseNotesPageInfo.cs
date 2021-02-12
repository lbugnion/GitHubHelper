using System.Collections.Generic;

namespace GitHubHelper
{
    public class ReleaseNotesPageInfo
    {
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
