using Newtonsoft.Json;
using System.Net;

namespace GitHubHelper.Model
{
    public class GetTextFileResult : ErrorResult
    {
        [JsonProperty("content")]
        public string EncodedContent
        {
            get;
            set;
        }

        [JsonProperty("html_url")]
        public string HtmlUrl
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty("path")]
        public string Path
        {
            get;
            set;
        }

        [JsonIgnore]
        public HttpStatusCode StatusCode
        {
            get;
            set;
        }

        [JsonIgnore]
        public string TextContent
        {
            get;
            set;
        }

        [JsonProperty("type")]
        public string Type
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