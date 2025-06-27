using MediatR;
using ShipConnect.UnitOfWorkContract;

namespace ShipConnect.ShippingCompanies.Commands
{
    public class DeleteShippingCompanyCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteShippingCompanyCommand(int id) => Id = id;
    }

    public class DeleteShippingCompanyHandler : IRequestHandler<DeleteShippingCompanyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteShippingCompanyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteShippingCompanyCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.ShippingCompanyRepository.GetByIdAsync(request.Id);
            if (entity == null) return false;

            await _unitOfWork.ShippingCompanyRepository.DeleteAsync(x => x.Id == request.Id);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }


}
