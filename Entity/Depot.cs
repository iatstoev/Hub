using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hub.Entity
{
    public class Depot:IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string BankName { get; set; }

        public decimal Balance { get; set; }

        public ICollection<Position> Position { get; set; }
    }
}
