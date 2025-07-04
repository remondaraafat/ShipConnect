

using Microsoft.EntityFrameworkCore;
using ShipConnect.DTOs.StartUpDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.StartUps.Commands
{
    public class EditStartupCommand:IRequest<bool>
    {
        
        public string Email { get; set; }
        public EditStartupDTO Data { get; set; }
    }
    public class EditStartupCommandHandler : IRequestHandler<EditStartupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public EditStartupCommandHandler(IUnitOfWork uow) => _unitOfWork = uow;

        public async Task<bool> Handle(EditStartupCommand request, CancellationToken cancellationToken)
        {
            StartUp entity = await _unitOfWork.StartUpRepository
            .GetWithFilterAsync(s => s.User.Email == request.Email).FirstOrDefaultAsync();
            
            if (entity == null)
            {
                return await Task.FromResult(false);
            }

            entity.Description = request.Data.Description;
            entity.BusinessCategory = request.Data.BusinessCategory;
            entity.Address = request.Data.Address;
            entity.Website = request.Data.Website;
            

             _unitOfWork.SaveAsync();
            return await Task.FromResult(true);
        }
    }
}
