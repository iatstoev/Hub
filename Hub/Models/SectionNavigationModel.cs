using Hub.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hub.Models
{
    public class SectionNavigationModel
    {
        
        public IEnumerable<Section> Sections {get; set;}

        public int SelectedSectionId { get; set; }

        public string SelectedSectionName { get; set; }

        public bool IsContentSection { get; set; }

        public string HtmlContent { get; set; }

        public int DeleteSectionId { get; set; }
    }
}