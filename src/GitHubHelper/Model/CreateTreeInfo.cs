using Newtonsoft.Json;
using System.Collections.Generic;

namespace GitHubHelper.Model
{
    public class CreateTreeInfo
    {
        [JsonProperty("base_tree")]
        public string BaseTree
        {
            get;
            set;
        }

        [JsonProperty("tree")]
        public IList<TreeInfo> Tree
        {
            get;
            private set;
        }

        public void AddTreeInfo(TreeInfo info)
        {
            if (Tree == null)
            {
                Tree = new List<TreeInfo>();
            }

            Tree.Add(info);
        }

        public void AddTreeInfos(IEnumerable<TreeInfo> infos)
        {
            foreach (var info in infos)
            {
                AddTreeInfo(info);
            }
        }
    }
}