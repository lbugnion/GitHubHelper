﻿using Newtonsoft.Json;

namespace GitHubHelper.Model
{
    public class NewBranchInfo : ShaInfo
    {
        public const string RefMask = "refs/heads/{0}";

        [JsonProperty("ref")]
        public string Ref
        {
            get;
            set;
        }
    }
}