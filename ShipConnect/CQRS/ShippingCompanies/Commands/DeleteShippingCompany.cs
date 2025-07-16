using MediatR;
using ShipConnect.Helpers;
using ShipConnect.UnitOfWorkContract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShipConnect.ShippingCompanies.Commands
{
    public class DeleteShippingCompanyCommand : IRequest<GeneralResponse<bool>>
    {
        public int Id { get; set; }
        public DeleteShippingCompanyCommand(int id) => Id = id;
    }

    public class DeleteShippingCompanyHandler : IRequestHandler<DeleteShippingCompanyCommand, GeneralResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteShippingCompanyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResponse<bool>> Handle(DeleteShippingCompanyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.ShippingCompanyRepository.GetByIdAsync(request.Id);
            if (entity == null)
                return GeneralResponse<bool>.FailResponse("Shipping company not found");

            if(!entity.User.IsApproved)
                return GeneralResponse<bool>.FailResponse("Company is already deleted");

            _unitOfWork.ShippingCompanyRepository.Update(entity);
            entity.User.IsApproved = false;

            await _unitOfWork.SaveAsync();

            return GeneralResponse<bool>.SuccessResponse("Shipping company deleted successfully", true);
        }
    }
}
