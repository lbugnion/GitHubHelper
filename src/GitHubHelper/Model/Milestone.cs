using Newtonsoft.Json;
using System;
using System.Globalization;

namespace GitHubHelper.Model
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
                    GitHubConstants.GitHubDateTimeFormat,
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
                    GitHubConstants.GitHubDateTimeFormat,
                    CultureInfo.InvariantCulture);
            }
        }
    }
}