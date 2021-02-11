using Newtonsoft.Json;

namespace GitHubHelper
{
    public class UpdateReferenceInfo : ShaInfo
    {
        [JsonProperty("force")]
        public bool Force => true;

        public UpdateReferenceInfo(string sha)
        {
            Sha = sha;
        }
    }
}