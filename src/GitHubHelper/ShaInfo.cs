using Newtonsoft.Json;

namespace GitHubHelper
{
    public class ShaInfo
    {
        public string ErrorMessage
        {
            get;
            set;
        }

        [JsonProperty("sha")]
        public string Sha
        {
            get;
            set;
        }
    }
}