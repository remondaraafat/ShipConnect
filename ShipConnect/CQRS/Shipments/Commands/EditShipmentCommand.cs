using MediatR;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Shipments.Commands
{
    public class EditShipmentCommand:IRequest<GeneralResponse<string>>
    {
        public string UserId { get; set; }
        public int ShipmentID { get; set; }

        public EditShipmentDTO DTO { get; set; }

        public EditShipmentCommand(string userId, int shipmentID, EditShipmentDTO dTO)
        {
            UserId = userId;
            ShipmentID = shipmentID;
            DTO = dTO;
        }
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

            shipment.WeightKg = request.DTO.WeightKg;
            shipment.Dimensions = request.DTO.Dimensions;
            shipment.Quantity = request.DTO.Quantity;
            shipment.Price = request.DTO.Price;
            shipment.PackagingOptions = request.DTO.PackagingOptions;
            shipment.DestinationAddress = request.DTO.DestinationAddress;
            shipment.TransportType = request.DTO.TransportType;
            shipment.ShippingScope = request.DTO.ShippingScope;
            shipment.Packaging = request.DTO.Packaging;
            shipment.Description = request.DTO.Description;
            shipment.ShipmentType = request.DTO.ShipmentType;
            shipment.RequestedPickupDate = request.DTO.RequestedPickupDate;
            shipment.SenderPhone = request.DTO.SenderPhone;
            shipment.SenderAddress = request.DTO.SenderAddress;
            shipment.SentDate = request.DTO.SentDate;
            receiver.FullName = request.DTO.RecipientName;
            receiver.Phone =request.DTO.RecipientEmail;
            receiver.Phone=request.DTO.RecipientPhone;            

            UnitOfWork.ShipmentRepository.Update(shipment);
            UnitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Shipment Edited successfully", shipment.Code);
        }
    }

}
