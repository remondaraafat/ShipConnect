using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Shipments.Commands
{
    public class DeleteShipmentCommand:IRequest<GeneralResponse<string>>    
    {
        public string UserID { get; set; }
        public int ShipmentId { get; set; }
    }
    public class DeleteShipmentCommandHandler : IRequestHandler<DeleteShipmentCommand, GeneralResponse<string>>
    {
        public IUnitOfWork UnitOfWork { get; }
        
        public DeleteShipmentCommandHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(DeleteShipmentCommand request, CancellationToken cancellationToken)
        {
            var startUp = await UnitOfWork.StartUpRepository.GetFirstOrDefaultAsync(s => s.UserId == request.UserID);
            if (startUp == null)
                return GeneralResponse<string>.FailResponse("StartUp not found");

            var shipment = await UnitOfWork.ShipmentRepository.GetFirstOrDefaultAsync(sh => sh.Id == request.ShipmentId && sh.StartupId== startUp.Id);
            if (shipment == null)
                return GeneralResponse<string>.FailResponse($"Shipment with Id {request.ShipmentId} not found");

            await UnitOfWork.ShipmentRepository.DeleteAsync(s => s.Id == request.ShipmentId && s.StartupId==startUp.Id);
            await UnitOfWork.SaveAsync();
            return GeneralResponse<string>.SuccessResponse("Shipment deleted successfuly");
        }
    }

}
