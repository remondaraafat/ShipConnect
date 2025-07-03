using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipConnect.Models;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class ShipmentDTO
    {
        //[Required(ErrorMessage = "*")]
        //[MaxLength(100)]
        //public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.1, double.MaxValue, ErrorMessage ="Weight must be greater than 0")]
        public double WeightKg { get; set; }  // وزن الشحنة بالكيلو

        [MaxLength(100)]
        public string? Dimensions { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
       
        [Required(ErrorMessage = "*")]
        [MaxLength (300)]
        public string DestinationAddress { get; set; }

        [Required(ErrorMessage = "*")]
        public PackagingOptions PackagingOptions { get; set; }
        //[Required(ErrorMessage = "*")]
        //[MaxLength(100)] 
        //public string DestinationCity { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(100)] 
        public string ShipmentType { get; set; }

        [Required(ErrorMessage = "*")]
        public TransportType TransportType { get; set; }
        
        [Required(ErrorMessage = "*")]
        public ShippingScope ShippingScope { get; set; }

        [MaxLength(100)]
        public string? Packaging { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "*")]
        public DateTime RequestedPickupDate { get; set; }//تاريخ الاستلام المطلوب

        //startUp data
        [Required(ErrorMessage = "*")]
        [Phone(ErrorMessage ="Invalid phone number")]
        public string SenderPhone { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(300)]
        public string SenderAddress { get; set; }//عنوان الارسال

        //[Required(ErrorMessage ="*")]
        //[MaxLength (100)]
        //public string SenderCity { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime SentDate { get; set; }//تاريخ الارسال

        //recipient data
        [Required(ErrorMessage = "*")]
        [MaxLength(150)]
        public string RecipientName { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        public string? RecipientEmail { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string RecipientPhone { get; set; } 

        //[MaxLength(300)]
        //public string? ReceiverNotes { get; set; }

        
    }
}


