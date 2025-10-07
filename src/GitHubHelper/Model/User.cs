using Newtonsoft.Json;

namespace GitHubHelper.Model
{
    public class User
    {
        [JsonProperty("login")]
        public string Login
        {
            get;
            set;
        }
    }
}