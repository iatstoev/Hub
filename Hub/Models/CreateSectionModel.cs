using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hub.Models
{
    public class CreateSectionModel
    {
        public int SectionParentID { get; set; }
        public string SectionName { get; set; }
        public bool IsContentSection { get; set; }
    }
}