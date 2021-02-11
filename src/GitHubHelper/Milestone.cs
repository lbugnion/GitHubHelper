using Newtonsoft.Json;
using System;
using System.Globalization;

namespace GitHubHelper
{
    public class Milestone
    {
        [JsonProperty("html_url")]
        public string Url
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        [JsonProperty("due_on")]
        public string DueOn
        {
            get;
            set;
        }

        [JsonIgnore]
        public DateTime DueOnLocal
        {
            get
            {
                if (string.IsNullOrEmpty(DueOn))
                {
                    return DateTime.MinValue;
                }

                return DateTime.ParseExact(
                    DueOn,
                    GitHubHelper.GitHubDateTimeFormat,
                    CultureInfo.InvariantCulture);
            }
        }

        [JsonProperty("closed_at")]
        public string ClosedAt
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
                    GitHubHelper.GitHubDateTimeFormat,
                    CultureInfo.InvariantCulture);
            }
        }
    }
}