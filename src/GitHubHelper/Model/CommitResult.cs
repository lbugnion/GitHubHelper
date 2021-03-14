using Newtonsoft.Json;

namespace GitHubHelper.Model
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