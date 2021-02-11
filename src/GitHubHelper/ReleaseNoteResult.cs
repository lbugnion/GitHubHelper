using System.Collections.Generic;

namespace GitHubHelper
{
    public class ReleaseNotesResult : ErrorResult
    {
        public IList<ReleaseNotesPageInfo> CreatedPages
        {
            get;
            set;
        }
    }
}
