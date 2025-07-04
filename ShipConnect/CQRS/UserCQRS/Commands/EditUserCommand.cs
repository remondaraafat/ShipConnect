using Microsoft.AspNetCore.Identity;
using ShipConnect.DTOs.UserDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.UserCQRS.Commands
{
    public class EditUserCommand:IRequest<IdentityResult>
    {
        public string Email { get; set; }
        public EditUserDTO DTO { get; set; }
    }
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, IdentityResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public EditUserCommandHandler(IUnitOfWork uow) => _unitOfWork = uow;

        public async Task<IdentityResult> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            
            ApplicationUser user = await _unitOfWork.ApplicationUserRepository.GetFirstOrDefaultAsync(u=> u.Email==request.Email);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found", Code = "UserNotFound" });


            user.PhoneNumber = request.DTO.Phone;
            user.Email = request.DTO.Email;
            user.Name = request.DTO.StartupName;
            user.ProfileImageUrl = request.DTO.ProfileImageUrl;



            await _unitOfWork.SaveAsync();
            return IdentityResult.Success;
        }
    }
}
