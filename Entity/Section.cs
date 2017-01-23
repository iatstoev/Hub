using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hub.Entity
{
    public class Section:IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        [ForeignKey("ParentSectionID")]
        public Section ParentSection { get; set; }
        
        public int? ParentSectionID { get; set; }

        public ICollection<Section> Sections { get; set; }

        [NotMapped]
        public int ChildrenCount { get; set; }
    }
}