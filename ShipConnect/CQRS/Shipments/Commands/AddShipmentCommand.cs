using MediatR;
using Microsoft.AspNetCore.Hosting;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Shipments.Commands
{
    public class AddShipmentCommand:IRequest<GeneralResponse<string>>
    {
        //public string Title { get; set; }
        public double WeightKg { get; set; }  // وزن الشحنة بالكيلو
        public string? Dimensions { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string DestinationAddress { get; set; }
        //public string DestinationCity { get; set; }
        public string ShipmentType { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? Packaging { get; set; }
        public PackagingOptions PackagingOptions { get; set; }
        public string? Description { get; set; }
        public DateTime RequestedPickupDate { get; set; }//تاريخ الاستلام المطلوب

        //startUp data
        public string SenderPhone { get; set; }
        public string SenderAddress { get; set; }//عنوان الارسال
        //public string SenderCity { get; set; }
        public DateTime SentDate { get; set; }//تاريخ الارسال

        //recipient data
        public string RecipientName { get; set; } = string.Empty;
        public string? RecipientEmail { get; set; }
        public string RecipientPhone { get; set; } = string.Empty;
        //public string? ReceiverNotes { get; set; }

        public string UserId { get; set; }
    }

    
    public class AddShipmentCommandHandler: IRequestHandler<AddShipmentCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IEmailService emailService;

        public AddShipmentCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            this.UnitOfWork = unitOfWork;
            this.emailService = emailService;
        }

        public async Task<GeneralResponse<string>> Handle(AddShipmentCommand request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserId);

            if (startUp == null)
                return GeneralResponse<string>.FailResponse("Startup not found for current user");

            Receiver receiverData = await UnitOfWork.ReceiverRepository.GetFirstOrDefaultAsync(r => r.Phone == request.RecipientPhone);

            if (receiverData==null)
            {
                receiverData = new Receiver
                {
                    FullName = request.RecipientName,
                    Phone = request.RecipientPhone,
                    Email = request.RecipientEmail,
                };
                await UnitOfWork.ReceiverRepository.AddAsync(receiverData);
                await UnitOfWork.SaveAsync();
            }


            var shipment = new Shipment
            {
                Code = $"SHIP-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                //Title = request.Title,
                WeightKg = request.WeightKg,
                Dimensions = request.Dimensions,
                Quantity = request.Quantity,
                Price = request.Price,
                //DestinationCity = request.DestinationCity,
                DestinationAddress = request.DestinationAddress,
                TransportType = request.TransportType,
                ShippingScope = request.ShippingScope,
                Packaging = request.Packaging,
                Description = request.Description,
                ShipmentType = request.ShipmentType,
                RequestedPickupDate = request.RequestedPickupDate,
                PackagingOptions = request.PackagingOptions,
                SenderPhone = request.SenderPhone,
                //SenderCity = request.SenderCity,
                SenderAddress = request.SenderAddress,
                SentDate = request.SentDate,
                //ReceiverNotes = request.ReceiverNotes,
                StartupId = startUp.Id,
                ReceiverId = receiverData.Id,
                Status = ShipmentStatus.Pending
            };

            await UnitOfWork.ShipmentRepository.AddAsync(shipment);
            await UnitOfWork.SaveAsync();


            //await emailService.SendEmailAsync(
            //    toEmail: startUp.User.Email,
            //    subject: "📦 Shipment Request Received - ShipConnect",
            //    body: $@"
            //    <div style='font-family: Arial, sans-serif; font-size: 16px;'>
            //        <p>Hi <strong>{startUp.CompanyName}</strong>,</p>
            //        <p>Your shipment request has been <strong>received successfully</strong>.</p>
            //        <p><strong>Shipment Code:</strong> <span style='color:#1a73e8;'>{shipment.Code}</span></p>
            //        <p>We'll notify you once a shipping company makes an offer.</p>
            //        <br/>
            //        <p style='font-size: 14px; color: #888;'>Thank you for using <strong>ShipConnect</strong>.</p>
            //    </div>"
            //);
            return GeneralResponse<string>.SuccessResponse("Shipment request submitted successfully", shipment.Code);
        }
    }

}
