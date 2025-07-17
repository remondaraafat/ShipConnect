using Microsoft.EntityFrameworkCore;
using ShipConnect.DTOs.OfferDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Payments.Queries
{
    public class GeStartUpPaymentsAmountQuery : IRequest<GeneralResponse<decimal>> 
    {
        public int? StartupId { get; }
        public string? UserId { get; }

        public GeStartUpPaymentsAmountQuery(int? startupId, string? userId)
        {
            StartupId = startupId;
            UserId = userId;
        }
    }

    public class GeStartUpPaymentsAmountQueryHandler : IRequestHandler<GeStartUpPaymentsAmountQuery, GeneralResponse<decimal>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GeStartUpPaymentsAmountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<decimal>> Handle(GeStartUpPaymentsAmountQuery request, CancellationToken cancellationToken)
        {
            string? payerId = request.UserId;

            if (request.StartupId.HasValue)
            {
                var startUp = await _unitOfWork.StartUpRepository
                    .GetFirstOrDefaultAsync(s => s.Id == request.StartupId.Value);

                if (startUp == null)
                    return GeneralResponse<decimal>.FailResponse("Startup not found");

                payerId = startUp.UserId;
            }

            if (string.IsNullOrEmpty(payerId))
                return GeneralResponse<decimal>.FailResponse("No identifier provided");

            var totalAmount = await _unitOfWork.PaymentRepository
                .GetWithFilterAsync(p => p.PayerId == payerId)
                .Select(p => p.Amount)
                .SumAsync(cancellationToken);

            return GeneralResponse<decimal>.SuccessResponse("Total amount retrieved successfully", totalAmount);
        }
    }
}


