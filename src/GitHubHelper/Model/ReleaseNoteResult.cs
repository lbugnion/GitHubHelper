using System.Collections.Generic;

namespace GitHubHelper.Model
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