using System.ComponentModel.DataAnnotations;

namespace ShipConnect.Models
{
    public class Receiver:BaseEntity
    {
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public string? City { get; set; }

        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
