using GitHubHelper.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GitHubHelper
{
    public class MilestoneComparer : IComparer<Milestone>
    {
        private bool _issuesAreClosed;

        public MilestoneComparer(bool issuesAreClosed)
        {
            _issuesAreClosed = issuesAreClosed;
        }

        public virtual int Compare(
            [AllowNull] 
            Milestone left, 
            [AllowNull] 
            Milestone right)
        {
            if (left == right)
            {
                return 0;
            }

            if (left == null)
            {
                return 1;
            }

            if (right == null)
            {
                return -1;
            }

            if (left.IsClosed
                && right.IsClosed)
            {
                return DateTime.Compare(left.ClosedLocal, right.ClosedLocal);
            }

            if (left.IsDue
                && right.IsDue)
            {
                return DateTime.Compare(left.DueOnLocal, right.DueOnLocal);
            }

            if (left.IsDue
                && !right.IsDue)
            {
                return 1;
            }

            if (!left.IsDue
                && right.IsDue)
            {
                return -1;
            }

            return string.Compare(left.Title, right.Title);
        }
    }
}