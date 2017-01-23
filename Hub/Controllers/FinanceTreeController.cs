using Hub.HubTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Hub.Controllers
{
    public class FinanceTreeController : Controller
    {
        private JsonResult ToJsonResult(object data)
        {
            JsonResult res = new JsonResult();
            res.Data = new JavaScriptSerializer().Serialize(data);
            return res;
        }

        public JsonResult InitializeTree()
        {
            FinanceTreeDataProvider provider = new FinanceTreeDataProvider();
            var treeData = provider.InitializeTreeData(null);

            JsonResult res = ToJsonResult(treeData);
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return res;
        }
    }
}