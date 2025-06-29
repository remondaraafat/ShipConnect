using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class CreateOfferCommand : IRequest<ReadOfferDto>
    {
        public CreateOfferDto Dto { get; }
        public CreateOfferCommand(CreateOfferDto dto) => Dto = dto;
    }

    public class CreateOfferHandler : IRequestHandler<CreateOfferCommand, ReadOfferDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOfferHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReadOfferDto> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = new Offer
            {
                Price = request.Dto.Price,
                EstimatedDeliveryDays = request.Dto.EstimatedDeliveryDays,
                Notes = request.Dto.Notes,
                ShipmentId = request.Dto.ShipmentId,
                ShippingCompanyId = request.Dto.ShippingCompanyId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.OfferRepository.AddAsync(offer);
            await _unitOfWork.SaveAsync();

            return new ReadOfferDto
            {
                Id = offer.Id,
                Price = offer.Price,
                EstimatedDeliveryDays = offer.EstimatedDeliveryDays,
                Notes = offer.Notes,
                IsAccepted = offer.IsAccepted,
                ShipmentId = offer.ShipmentId,
                ShippingCompanyId = offer.ShippingCompanyId
            };
        }
    }

}
