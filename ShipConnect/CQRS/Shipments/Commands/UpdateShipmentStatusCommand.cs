
using ShipConnect.DTOs.ShipmentDTOs;

namespace ShipConnect.CQRS.Shipments.Commands
{
    public class UpdateShipmentStatusCommand:IRequest<GeneralResponse<string>>
    {
        public string UserId { get; set; }
        public int ShipmentId { get; set; }
        public int ShipmentStatus { get; set; }
    }

    public class UpdateShipmentStatusCommandHandler : IRequestHandler<UpdateShipmentStatusCommand,GeneralResponse<string>>
    {
        public IUnitOfWork UnitOfWork { get; }
        public UpdateShipmentStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
        {
            var company = await UnitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(s=>s.UserId==request.UserId);   
            if (company == null)
                return GeneralResponse<string>.FailResponse("Unauthorized user");

            var newStatus = (ShipmentStatus)request.ShipmentStatus;

            if (!Enum.IsDefined(typeof(ShipmentStatus), newStatus))//بيتاكد هل القيمة موجودة فعليا ولا
                return GeneralResponse<string>.FailResponse("Invalid shipment status value");
            
            var shipment = UnitOfWork.OfferRepository
                                    .GetWithFilterAsync(o => o.ShippingCompanyId == company.Id
                                    && o.ShipmentId == request.ShipmentId
                                    && o.IsAccepted == true).Select(o=>o.Shipment).FirstOrDefault();

            if (shipment == null)
                return GeneralResponse<string>.FailResponse("Shipment not found");

            shipment.Status = newStatus;
            await UnitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Shipment status Updated successfully");
        }
    }
}
