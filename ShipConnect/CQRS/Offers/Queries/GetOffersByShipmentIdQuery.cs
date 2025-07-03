using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetOffersByShipmentIdQuery : IRequest<GeneralResponse<List<ReadOfferDto>>>
    {
        public int ShipmentId { get; set; }
        public GetOffersByShipmentIdQuery(int shipmentId) => ShipmentId = shipmentId;
    }

    public class GetOffersByShipmentIdHandler : IRequestHandler<GetOffersByShipmentIdQuery, GeneralResponse<List<ReadOfferDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOffersByShipmentIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<List<ReadOfferDto>>> Handle(GetOffersByShipmentIdQuery request, CancellationToken cancellationToken)
        {
            

            var data = _unitOfWork.OfferRepository
                .GetWithFilterAsync(o => o.ShipmentId == request.ShipmentId).Select(o => new ReadOfferDto
            {
                Id = o.Id,
                Price = o.Price,
                EstimatedDeliveryDays = o.EstimatedDeliveryDays,
                Notes = o.Notes,
                IsAccepted = o.IsAccepted,
                ShipmentId = o.ShipmentId,
                ShippingCompanyId = o.ShippingCompanyId
            }).ToList();

            return GeneralResponse<List<ReadOfferDto>>.SuccessResponse("Offers retrieved successfully", data);
        }
    }
}
