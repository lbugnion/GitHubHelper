using Newtonsoft.Json;
using System.Collections.Generic;

namespace GitHubHelper.Model
{
    public class CommitInfo
    {
        [JsonProperty("message")]
        public string Message
        {
            get;
        }

        [JsonProperty("parents")]
        public IList<string> Parents
        {
            get;
        }

        [JsonProperty("tree")]
        public string Tree
        {
            get;
        }

        public CommitInfo(string message, string parentTreeSha, string treeSha)
        {
            Message = message;
            Parents = new List<string>
            {
                parentTreeSha
            };
            Tree = treeSha;
        }
    }
}