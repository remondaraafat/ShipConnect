using MediatR;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Helpers;
using ShipConnect.Models;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.CQRS.Offers.Commands
{
    // command
    public class CreateOfferCommand : IRequest<GeneralResponse<ReadOfferDto>>
    {
        public CreateOfferDto Dto { get; }
        public CreateOfferCommand(CreateOfferDto dto) => Dto = dto;
    }

    // handler
    public class CreateOfferHandler : IRequestHandler<CreateOfferCommand, GeneralResponse<ReadOfferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOfferHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<ReadOfferDto>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            try
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

                return GeneralResponse<ReadOfferDto>.SuccessResponse("Offer created successfully", dto);
            }
            catch (Exception ex)
            {
                return GeneralResponse<ReadOfferDto>.FailResponse($"Failed to create offer: {ex.Message}");
            }
        }
    }
}
