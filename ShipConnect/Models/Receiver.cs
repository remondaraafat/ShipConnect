using System.ComponentModel.DataAnnotations;

namespace ShipConnect.Models
{
    public class Receiver:BaseEntity
    {
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
