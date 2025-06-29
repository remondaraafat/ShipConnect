using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Queries
{
    public class GetOffersByShipmentIdQuery : IRequest<List<ReadOfferDto>>
    {
        public int ShipmentId { get; set; }
        public GetOffersByShipmentIdQuery(int shipmentId) => ShipmentId = shipmentId;
    }

    public class GetOffersByShipmentIdHandler : IRequestHandler<GetOffersByShipmentIdQuery, List<ReadOfferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOffersByShipmentIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ReadOfferDto>> Handle(GetOffersByShipmentIdQuery request, CancellationToken cancellationToken)
        {
            var offers = await _unitOfWork.OfferRepository
                .GetWithFilterAsync(o => o.ShipmentId == request.ShipmentId);

            return offers.Select(o => new ReadOfferDto
            {
                Id = o.Id,
                Price = o.Price,
                EstimatedDeliveryDays = o.EstimatedDeliveryDays,
                Notes = o.Notes,
                IsAccepted = o.IsAccepted,
                ShipmentId = o.ShipmentId,
                ShippingCompanyId = o.ShippingCompanyId
            }).ToList();
        }
    }

}
