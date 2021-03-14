using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GitHubHelper.Model
{
    public class IssueInfo
    {
        [JsonProperty("html_url")]
        public string Url
        {
            get;
            set;
        }

        public int Number
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public IssueState State
        {
            get;
            set;
        }

        [JsonIgnore]
        public IList<string> Projects
        {
            get;
            set;
        }

        public IList<IssueLabel> Labels
        {
            get;
            set;
        }

        public Milestone Milestone
        {
            get;
            set;
        }

        [JsonProperty("pull_request")]
        public PullRequest PullRequest
        {
            get;
            set;
        }

        [JsonProperty("closed_at")]
        public string ClosedAt
        {
            get;
            set;
        }

        public DateTime ClosedLocal
        {
            get
            {
                if (string.IsNullOrEmpty(ClosedAt))
                {
                    return DateTime.MinValue;
                }

                return DateTime.ParseExact(
                    ClosedAt,
                    GitHubConstants.GitHubDateTimeFormat,
                    CultureInfo.InvariantCulture);
            }
        }

        public override string ToString()
        {
            return $"{Number:D2} / {State} / {PullRequest == null} / {Title}";
        }
    }
}
