using Newtonsoft.Json;

namespace GitHubHelper.Model
{
    public class IssueLabel
    {
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
    }
}