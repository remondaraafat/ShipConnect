using System.Security.Cryptography;
using System.Text;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Shipments.Commands
{
    public class AddShipmentCommand:IRequest<GeneralResponse<string>>
    {
        public string UserId { get;}
        public AddShipmentDTO DTO { get;}

        public AddShipmentCommand(string UserId, AddShipmentDTO dto)
        {
            this.UserId = UserId;
            this.DTO = dto;
        }
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
                return GeneralResponse<string>.FailResponse("User not found");

            var receiverData = await UnitOfWork.ReceiverRepository.GetFirstOrDefaultAsync(r => r.Phone == request.DTO.RecipientPhone);

            if (receiverData==null)
            {
                receiverData = new Receiver
                {
                    FullName = request.DTO.RecipientName,
                    Phone = request.DTO.RecipientPhone,
                    Email = request.DTO.RecipientEmail,
                };
                await UnitOfWork.ReceiverRepository.AddAsync(receiverData);
                await UnitOfWork.SaveAsync();
            }

            var shipment = new Shipment
            {
                Code = $"SHIP-{GenerateRandomCode(8)}",
                WeightKg = request.DTO.WeightKg,
                Dimensions = request.DTO.Dimensions,
                Quantity = request.DTO.Quantity,
                Price = request.DTO.Price,
                DestinationAddress = request.DTO.DestinationAddress,
                TransportType = request.DTO.TransportType,
                ShippingScope = request.DTO.ShippingScope,
                Packaging = request.DTO.Packaging,
                Description = request.DTO.Description,
                ShipmentType = request.DTO.ShipmentType,
                RequestedPickupDate = request.DTO.RequestedPickupDate,
                PackagingOptions = request.DTO.PackagingOptions,
                SenderPhone = request.DTO.SenderPhone,
                SenderAddress = request.DTO.SenderAddress,
                SentDate = request.DTO.SentDate,
                StartupId = startUp.Id,
                ReceiverId = receiverData.Id,
                Status = ShipmentStatus.Pending
            };

            await UnitOfWork.ShipmentRepository.AddAsync(shipment);
            await UnitOfWork.SaveAsync();

            var user = UnitOfWork.StartUpRepository.GetWithFilterAsync(s => s.UserId == request.UserId).Select(s => s.User).FirstOrDefault();

            await emailService.SendEmailAsync(
                toEmail: user.Email,
                subject: "📦 Shipment Request Received - ShipConnect",
                body: $@"
                <div style='font-family: Arial, sans-serif; font-size: 16px; color: #333;'>
                    <p>Dear <strong>{startUp.CompanyName}</strong>,</p>
                    <p>We are pleased to inform you that your shipment request has been <strong>successfully received</strong>.</p>
                    <p>
                        <strong>Shipment Code:</strong> 
                        <span style='color: #1a73e8; font-weight: bold;'>{shipment.Code}</span>
                    </p>
                    <p>Our team will notify you as soon as a shipping company submits an offer for this shipment.</p>
                    <p>If you have any questions or need assistance, feel free to reach out to our support team at any time.</p>
                    <br/>
                    <p style='font-size: 14px; color: #888;'>Thank you for choosing <strong>ShipConnect</strong>. We appreciate your trust.</p>
                </div>"

            );
            return GeneralResponse<string>.SuccessResponse("Shipment request submitted successfully", shipment.Code);
        }

        private static string GenerateRandomCode(int length)
        {
            Span<byte> buffer = stackalloc byte[length];
            RandomNumberGenerator.Fill(buffer);

            // تحويل إلى Base32 (A-Z2-7) لضغط الحروف
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var sb = new StringBuilder(length);

            foreach (byte b in buffer)
                sb.Append(alphabet[b % alphabet.Length]);

            return sb.ToString();
        }
    }


}
