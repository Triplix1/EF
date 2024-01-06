using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopReports.Models
{
    [Table("supermarket_locations")]
    public class SupermarketLocation
    {
        [Key]
        [Column("supermarket_location_id")]
        public int Id { get; set; }

        [Column("supermarket_id")]
        [ForeignKey(nameof(Supermarket))]
        public int SupermarketId { get; set; }

        [Column("location_id")]
        [ForeignKey(nameof(Location))]
        public int LocationId { get; set; }

        public Supermarket Supermarket { get; set; }

        public Location Location { get; set; }

        public virtual IList<Order> Orders { get; set; }
    }
}
