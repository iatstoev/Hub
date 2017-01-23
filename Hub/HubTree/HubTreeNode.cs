using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hub.HubTree
{

    public class HubTreeNodeContent
    {
        public string Url { get; set; }
    }


    public class HubTreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasChildren { get; set; }
        public HubTreeNodeContent Content { get; set; } 
    }
}