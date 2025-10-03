using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GitHubHelper.Model
{
    public class IssueInfo
    {
        [JsonProperty("created_at")]
        public string CreatedAt
        {
            get;
            set;
        }

        [JsonProperty("updated_at")]
        public string UpdatedAt
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

        public int Number
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

        [JsonProperty("pull_request")]
        public PullRequest PullRequest
        {
            get;
            set;
        }

        public IssueState State
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        [JsonProperty("html_url")]
        public string Url
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Number:D2} / {State} / {PullRequest == null} / {Title}";
        }
    }
}