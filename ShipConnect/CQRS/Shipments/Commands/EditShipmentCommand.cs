using MediatR;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Shipments.Commands
{
    public class EditShipmentCommand:IRequest<GeneralResponse<string>>
    {
        public int ShipmentID { get; set; }
        public string ShipmentType { get; set; }
        public double WeightKg { get; set; }  // وزن الشحنة بالكيلو
        public string? Dimensions { get; set; }
        public int Quantity { get; set; }
        public DateTime SentDate { get; set; }//تاريخ الارسال
        public DateTime RequestedPickupDate { get; set; }//تاريخ الاستلام المطلوب
        public decimal Price { get; set; }


        //public string Title { get; set; }
        public string DestinationAddress { get; set; }
        //public string DestinationCity { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? Packaging { get; set; }
        public PackagingOptions PackagingOptions { get; set; }
        public string? Description { get; set; }

        //startUp data
        public string SenderPhone { get; set; }
        public string SenderAddress { get; set; }//عنوان الارسال
        //public string SenderCity { get; set; }

        //recipient data
        public string RecipientName { get; set; } = string.Empty;
        public string? RecipientEmail { get; set; }
        public string RecipientPhone { get; set; } = string.Empty;
        //public string? ReceiverNotes { get; set; }

        public string UserId { get; set; }
    }

    public class EditShipmentCommandHandler : IRequestHandler<EditShipmentCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork UnitOfWork;

        public EditShipmentCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(EditShipmentCommand request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (startUp == null)
                return GeneralResponse<string>.FailResponse("Startup not found for current user");
            
            var shipment = await UnitOfWork.ShipmentRepository.GetByIdAsync(request.ShipmentID);
            if (shipment == null||shipment.StartupId!=startUp.Id)
                return GeneralResponse<string>.FailResponse("Shipment not found or access denied");
            var receiver = await UnitOfWork.ReceiverRepository.GetFirstOrDefaultAsync(r => r.Id == shipment.ReceiverId);
            if (receiver == null)
                return GeneralResponse<string>.FailResponse("Receiver Data not found");

            //shipment.Title = request.Title;
            shipment.WeightKg = request.WeightKg;
            shipment.Dimensions = request.Dimensions;
            shipment.Quantity = request.Quantity;
            shipment.Price = request.Price;
            //shipment.DestinationCity = request.DestinationCity;
            shipment.PackagingOptions = request.PackagingOptions;
            shipment.DestinationAddress = request.DestinationAddress;
            shipment.TransportType = request.TransportType;
            shipment.ShippingScope = request.ShippingScope;
            shipment.Packaging = request.Packaging;
            shipment.Description = request.Description;
            shipment.ShipmentType = request.ShipmentType;
            shipment.RequestedPickupDate = request.RequestedPickupDate;
            shipment.SenderPhone = request.SenderPhone;
            //shipment.SenderCity = request.SenderCity;
            shipment.SenderAddress = request.SenderAddress;
            shipment.SentDate = request.SentDate;
            //shipment.ReceiverNotes = request.ReceiverNotes;
            receiver.FullName = request.RecipientName;
            receiver.Phone =request.RecipientEmail;
            receiver.Phone=request.RecipientPhone;            

            UnitOfWork.ShipmentRepository.Update(shipment);
            UnitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Shipment Edited successfully", shipment.Code);
        }
    }

}
