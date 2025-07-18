using Microsoft.AspNetCore.Identity;
using ShipConnect.DTOs.UserDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.UserCQRS.Commands
{
    public class EditUserCommand:IRequest<IdentityResult>
    {
        public string Id { get; set; }
        public EditUserDTO DTO { get; set; }

        public EditUserCommand(string userId, EditUserDTO dto)
        {
            this.Id = userId;
            this.DTO = dto;            
        }
    }
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, IdentityResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public EditUserCommandHandler(IUnitOfWork uow) => _unitOfWork = uow;

        public async Task<IdentityResult> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _unitOfWork.ApplicationUserRepository.GetFirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found", Code = "UserNotFound" });

            
            if (request.DTO.ProfileImageFile != null)
            {
                var fileName = await FileHelper.UploadFileAsync(request.DTO.ProfileImageFile);
                if (fileName.Contains("MB") || fileName.Contains("Null"))
                    return IdentityResult.Failed(new IdentityError { Description = fileName });

                user.ProfileImageUrl = Path.Combine("images", fileName);
            }

           
            

            await _unitOfWork.SaveAsync();

            return IdentityResult.Success;
        }
    }
}
