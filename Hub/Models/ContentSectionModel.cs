using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hub.Models
{
    public enum ContentSectionAction
    {
        DISPLAY,
        EDIT
    }

    public class ContentSectionModel
    {
        public int SectionId { get; set; }
        [AllowHtml]
        public string HtmlContent { get; set; }
        public ContentSectionAction Action { get; set; }
    }
}