using Newtonsoft.Json;

namespace GitHubHelper
{
    public class CommitResult : ShaInfo
    {
        [JsonProperty("tree")]
        public ShaInfo Tree
        {
            get;
            set;
        }
    }
}