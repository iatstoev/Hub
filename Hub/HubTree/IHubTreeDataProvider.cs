using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hub.HubTree
{
    interface IHubTreeDataProvider
    {
        /// <summary>
        /// Provide the initial list of tree nodes that are to be rendered
        /// </summary>
        /// <returns></returns>
        IEnumerable<HubTreeNode> InitializeTreeData(int? rootNode);

        HubTreeNode AddSiblingNode(int nodeId);

        HubTreeNode AddChildNode(int nodeId);

        IEnumerable<HubTreeNode> DeleteNodeReattachChildren(int nodeId);

        void RenameNode(int nodeId, string name);

        HubTreeNode TransformToContentNode(int nodeId);
    }
}
