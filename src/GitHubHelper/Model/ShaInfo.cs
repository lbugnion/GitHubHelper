using Newtonsoft.Json;

namespace GitHubHelper.Model
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