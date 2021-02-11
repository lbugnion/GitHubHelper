using System.Collections.Generic;

namespace GitHubHelper
{
    public class IssueResult : ErrorResult
    {
        public IList<IssueInfo> Issues
        {
            get;
            set;
        }
    }
}