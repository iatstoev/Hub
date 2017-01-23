using Hub.Entity;
using Hub.Models;
using Hub.Services;
using System.Web.Mvc;

namespace Hub.Controllers
{
    [Authorize]
    public class SectionController:Controller
    {
        private SectionService SectionService;

        public SectionController()
        {
            SectionService = new SectionService();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            //default and empty            
            return View();
        }


        public PartialViewResult DisplayContentSection(int nodeId)
        {
            SectionService sService = new SectionService();
            var section = sService.GetEntity(nodeId) as ContentSection;

            ContentSectionModel csm = new ContentSectionModel();
            csm.Action = ContentSectionAction.DISPLAY;
            csm.SectionId = nodeId;
            csm.HtmlContent = section.HtmlContent;
            return PartialView("ContentSection", csm);
        }


        public PartialViewResult EditContentSection(int nodeId)
        {
            SectionService sService = new SectionService();
            var section = sService.GetEntity(nodeId) as ContentSection;

            ContentSectionModel csm = new ContentSectionModel();
            csm.Action = ContentSectionAction.EDIT;
            csm.SectionId = nodeId;
            csm.HtmlContent = section.HtmlContent;
            return PartialView("ContentSection", csm);
        }

        [HttpPost]
        public PartialViewResult SaveContentSection(ContentSectionModel model)
        {
            SectionService sService = new SectionService();
            var section = sService.GetEntity(model.SectionId) as ContentSection;
            section.HtmlContent = model.HtmlContent;
            sService.SaveChanges();

            return DisplayContentSection(model.SectionId);
        }
    }

}