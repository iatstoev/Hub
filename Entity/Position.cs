using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hub.Entity
{
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("DepotID")]
        public Depot Depot { get; set; }

        public int DepotID { get; set; }

        public string Description { get; set; }

        public decimal CurrentValue { get; set; }
        
    }
}
