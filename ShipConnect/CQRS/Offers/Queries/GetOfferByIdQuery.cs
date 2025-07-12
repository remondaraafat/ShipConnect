using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetOfferByIdQuery : IRequest<GeneralResponse<ReadOfferDto>>
    {
        public string UserId { get;}
        public int Id { get; }

        public GetOfferByIdQuery(string userId, int id)
        {
            UserId = userId;
            Id = id;
        }
    }

    public class GetOfferByIdHandler : IRequestHandler<GetOfferByIdQuery, GeneralResponse<ReadOfferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOfferByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ReadOfferDto>> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.ShippingCompanyRepository.GetFirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (company == null)
                return GeneralResponse<ReadOfferDto>.FailResponse("User not found");

            var offer = await _unitOfWork.OfferRepository.GetFirstOrDefaultAsync(o=>o.Id==request.Id && o.ShippingCompanyId==company.Id);
            if (offer == null)
                return GeneralResponse<ReadOfferDto>.FailResponse("Offer not found");

            var dto = new ReadOfferDto
            {
                Id = offer.Id,
                Price = offer.Price,
                EstimatedDeliveryDays = offer.EstimatedDeliveryDays,
                Notes = offer.Notes,
                IsAccepted = offer.IsAccepted,
                ShipmentId = offer.ShipmentId,
                ShippingCompanyId = offer.ShippingCompanyId
            };

            return GeneralResponse<ReadOfferDto>.SuccessResponse("Offer retrieved successfully", dto);
        }
    }
}
