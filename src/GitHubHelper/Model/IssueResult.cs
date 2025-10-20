using System.Collections.Generic;

namespace GitHubHelper.Model
{
    public class IssueResult : ErrorResult
    {
        public IList<IssueInfo> Issues
        {
            get;
            set;
        }

        public string Json
        {
            get;
            set;
        }
    }
}