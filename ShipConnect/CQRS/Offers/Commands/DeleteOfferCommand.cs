using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class DeleteOfferCommand : IRequest<GeneralResponse<string>>
    {
        public int Id { get; set; }
        public DeleteOfferCommand(int id) => Id = id;
    }

    public class DeleteOfferHandler : IRequestHandler<DeleteOfferCommand, GeneralResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOfferHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<string>> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _unitOfWork.OfferRepository.GetByIdAsync(request.Id);
            if (offer == null)
            {
                return GeneralResponse<string>.FailResponse("Offer not found");
            }

            await _unitOfWork.OfferRepository.DeleteAsync(o => o.Id == request.Id);
            await _unitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Offer deleted successfully");
        }
    }
}
