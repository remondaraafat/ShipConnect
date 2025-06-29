using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    public class UpdateOfferCommand : IRequest<ReadOfferDto?>
    {
        public int Id { get; set; }
        public UpdateOfferDto Dto { get; set; }
        public UpdateOfferCommand(int id, UpdateOfferDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }

    public class UpdateOfferHandler : IRequestHandler<UpdateOfferCommand, ReadOfferDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOfferHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReadOfferDto?> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _unitOfWork.OfferRepository.GetByIdAsync(request.Id);
            if (offer == null) return null;

            offer.Price = request.Dto.Price;
            offer.EstimatedDeliveryDays = request.Dto.EstimatedDeliveryDays;
            offer.Notes = request.Dto.Notes;
            offer.IsAccepted = request.Dto.IsAccepted;

            _unitOfWork.OfferRepository.Update(offer);
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
