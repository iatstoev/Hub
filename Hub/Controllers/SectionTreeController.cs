using Hub.HubTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Hub.Controllers
{
    [Authorize]
    public class SectionTreeController : Controller
    {

        class ContextMenuItem
        {
            public string ContextActionName { get; set; }
            public string Url { get; set; }
        }

        private string GenerateUrl(string Action, string Controller)
        {
            return UrlHelper.GenerateUrl(null, Action, Controller, null, RouteTable.Routes, HttpContext.Request.RequestContext, false);
        }

        public JsonResult ContextMenuUrls()
        {
            Dictionary<string, string> cmItems = new Dictionary<string, string>();

            cmItems.Add("hubTreeNodeExpansionUrl", GenerateUrl("ExpandNode", "SectionTree"));
            cmItems.Add("bindAddSiblingNode", GenerateUrl("AddSiblingNode", "SectionTree"));
            cmItems.Add("bindAddChildNode", GenerateUrl("AddChildNode", "SectionTree"));
            cmItems.Add("bindRenameNode", GenerateUrl("RenameNode", "SectionTree"));
            cmItems.Add("bindDeleteNode", GenerateUrl("DeleteNodeReattachChildren", "SectionTree"));
            cmItems.Add("bindToContentNode", GenerateUrl("ToContentNode", "SectionTree"));

            JsonResult res = ToJsonResult(cmItems);
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return res;
        }

        private JsonResult ToJsonResult(object data)
        {
            JsonResult res = new JsonResult();
            res.Data =  new JavaScriptSerializer().Serialize(data);
            return res;
        }

        public JsonResult InitializeTree(int? rootNode = null)
        {
            SectionTreeDataProvider provider = new SectionTreeDataProvider();
            var treeData = provider.InitializeTreeData(rootNode);


            JsonResult res = ToJsonResult(treeData);
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }

        public JsonResult ExpandNode(int nodeId)
        {
            return InitializeTree(nodeId);
        }

        [HttpPut]
        public JsonResult AddSiblingNode(int nodeId)
        {
            SectionTreeDataProvider provider = new SectionTreeDataProvider();
            return ToJsonResult(provider.AddSiblingNode(nodeId));
        }
        
        [HttpPost]
        public JsonResult AddChildNode(int nodeId)
        {
            SectionTreeDataProvider provider = new SectionTreeDataProvider();
            return ToJsonResult(provider.AddChildNode(nodeId));
        }

        [HttpPost]
        public JsonResult RenameNode(int nodeId, string name)
        {
            SectionTreeDataProvider provider = new SectionTreeDataProvider();
            provider.RenameNode(nodeId, name);

            return ToJsonResult(new List<object>());
        }

        [HttpPost]
        public JsonResult DeleteNodeReattachChildren(int nodeId)
        {
            SectionTreeDataProvider provider = new SectionTreeDataProvider();
            return ToJsonResult(provider.DeleteNodeReattachChildren(nodeId));
        }

        [HttpPost]
        public JsonResult ToContentNode(int nodeId)
        {
            SectionTreeDataProvider provider = new SectionTreeDataProvider();
            return ToJsonResult(provider.TransformToContentNode(nodeId));
        }
    }
}