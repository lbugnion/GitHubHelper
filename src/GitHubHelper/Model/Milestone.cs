using Newtonsoft.Json;
using System;
using System.Globalization;

namespace GitHubHelper.Model
{
    public class Milestone
    {
        [JsonProperty("closed_at")]
        public string ClosedAt
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

        [JsonIgnore]
        public bool IsClosed
        {
            get
            {
                return ClosedLocal >= DateTime.MinValue + TimeSpan.FromDays(1);
            }
        }

        [JsonIgnore]
        public bool IsDue
        {
            get
            {
                return DueOnLocal >= DateTime.MinValue + TimeSpan.FromDays(1);
            }
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
    }
}