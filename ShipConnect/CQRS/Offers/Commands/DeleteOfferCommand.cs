using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class DeleteOfferCommand : IRequest<GeneralResponse<string>>
    {
        public string UserId { get;}
        public int OfferId { get;}

        public DeleteOfferCommand(string userId, int id)
        {
            UserId = userId;
            OfferId = id;
        }
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
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (company == null)
                return GeneralResponse<string>.FailResponse("Unauthorized user");

            var offer = await _unitOfWork.OfferRepository
                                        .GetFirstOrDefaultAsync(o=>o.Id==request.OfferId 
                                        && o.ShippingCompanyId==company.Id
                                        && !o.IsAccepted);

            if (offer == null)
                return GeneralResponse<string>.FailResponse("Offer not found or offer already accepted");

            await _unitOfWork.OfferRepository
                            .DeleteAsync(o => o.Id == request.OfferId
                            && o.ShippingCompanyId == company.Id
                            && !o.IsAccepted);

            await _unitOfWork.SaveAsync();

            return GeneralResponse<string>.SuccessResponse("Offer deleted successfully");
        }
    }
}
