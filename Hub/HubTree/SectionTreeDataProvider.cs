using Hub.Entity;
using Hub.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hub.HubTree
{

    public class SectionTreeDataProvider:IHubTreeDataProvider
    {
        SectionService SectionService;

        private static HubTreeNode ToHubTreeNode(Section section)
        {
            HubTreeNode node = new HubTreeNode() { Id = section.ID, Name = section.Name, HasChildren = section.ChildrenCount > 0 };
            if (section is ContentSection)
            {
                node.Content = new HubTreeNodeContent();

                //
                //theoretically we could get some node specific url here depending
                node.Content.Url = UrlHelper.GenerateUrl(null, "DisplayContentSection", "Section", null,RouteTable.Routes, HttpContext.Current.Request.RequestContext, false); 
            }
            return node;
        }

        public SectionTreeDataProvider()
        {
            SectionService = new SectionService();
        }

        public IEnumerable<HubTreeNode> InitializeTreeData(int? rootNode)
        {
            List<HubTreeNode> nodeList = new List<HubTreeNode>();
            var sections = SectionService.LoadChildSectionsForParent(rootNode);

            foreach(var section in sections)
                nodeList.Add(ToHubTreeNode(section));

            return nodeList;
        }

        public IEnumerable<HubTreeNode> LoadSectionsForParent(int parentNode)
        {
            return InitializeTreeData(parentNode);
        }

        public HubTreeNode AddSiblingNode(int nodeId)
        {
            var node = SectionService.GetEntity(nodeId);
            var newSection = SectionService.CreateEmptySection(node.ParentSectionID);
            return ToHubTreeNode(newSection);
        }

        public HubTreeNode AddChildNode(int nodeId)
        {
            var newSection = SectionService.CreateEmptySection(nodeId);
            return ToHubTreeNode(newSection);
        }

        public void RenameNode(int nodeId, string name)
        {
            SectionService.RenameSection(nodeId, name);
        }

        public IEnumerable<HubTreeNode> DeleteNodeReattachChildren(int nodeId)
        {
            //
            //get the node's children
            //those are not attached to the EF!
            var children = SectionService.LoadChildSectionsForParent(nodeId);
            SectionService.DeleteSectionWithoutChildren(nodeId);

            List<HubTreeNode> treeChildren = new List<HubTreeNode>();

            if (children != null)
            {
                foreach (var child in children)
                    treeChildren.Add(ToHubTreeNode(child));
            }

            return treeChildren;
        }

        public HubTreeNode TransformToContentNode(int nodeId)
        {
            //this is a rather bullshit function that will change the entity type for the section node from section to content section
            //this is not supported by the entity framework and for good reasons. 
            //alternative later when implementing a module tree is to use composition over inheritance. for example a section may have a content entity and here we would simply add the content.
            SectionService.TransformSectionToContentSection(nodeId);

            return ToHubTreeNode(SectionService.GetEntity(nodeId));
        }
    }
}