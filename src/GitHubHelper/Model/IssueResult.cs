using System.Collections.Generic;

namespace GitHubHelper.Model
{
    public class IssueResult : ErrorResult
    {
        public List<IssueInfo> Issues
        {
            get;
            set;
        } = [];

        public List<string> JsonFiles
        {
            get;
            set;
        } = [];
    }
}