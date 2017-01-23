using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hub.HubTree
{
    public class FinanceTreeDataProvider:IHubTreeDataProvider
    {
        //private static HubTreeNode ToHubTreeNode(Section section)
        //{
        //    HubTreeNode node = new HubTreeNode() { Id = section.ID, Name = section.Name, HasChildren = section.ChildrenCount > 0 };
        //    if (section is ContentSection)
        //    {
        //        node.Content = new HubTreeNodeContent();

        //        //
        //        //theoretically we could get some node specific url here depending
        //        node.Content.Url = UrlHelper.GenerateUrl(null, "DisplayContentSection", "Section", null, RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);
        //    }
        //    return node;
        //}

        public FinanceTreeDataProvider()
        {
        }

        public IEnumerable<HubTreeNode> InitializeTreeData(int? rootNode)
        {
            List<HubTreeNode> nodeList = new List<HubTreeNode>();

            HubTreeNode node = new HubTreeNode() { Id = 1, Name = "Accounts", HasChildren = false};
            node.Content = new HubTreeNodeContent();
            node.Content.Url = UrlHelper.GenerateUrl(null, "DisplayAccounts", "Finance", null, RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);

            nodeList.Add(node);

            node = new HubTreeNode() { Id = 2, Name = "Depots", HasChildren = false };
            node.Content = new HubTreeNodeContent();
            node.Content.Url = UrlHelper.GenerateUrl(null, "DisplayDepots", "Finance", null, RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);

            nodeList.Add(node);

            return nodeList;
        }

        public IEnumerable<HubTreeNode> LoadSectionsForParent(int parentNode)
        {
            return InitializeTreeData(parentNode);
        }

        //
        //No context menu for this provider

        public HubTreeNode AddSiblingNode(int nodeId)
        {
            throw new  NotImplementedException();
        }

        public HubTreeNode AddChildNode(int nodeId)
        {
            throw new NotImplementedException();
        }

        public void RenameNode(int nodeId, string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HubTreeNode> DeleteNodeReattachChildren(int nodeId)
        {
            throw new NotImplementedException();
        }

        public HubTreeNode TransformToContentNode(int nodeId)
        {
            throw new NotImplementedException();
        }
    }
}