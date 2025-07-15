using Microsoft.AspNetCore.Identity;
using ShipConnect.DTOs.AccountDTOs;
using ShipConnect.DTOs.ShipmentDTOs;
using ShipConnect.Models;

namespace ShipConnect.CQRS.Register.Queries
{
    public class GetPendingAccountsQuery:IRequest<GeneralResponse<GetDataResult<List<PendingAccountDTO>>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPendingAccountsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;            
        }
    }

    public class GetPendingAccountsQueryHandler:IRequestHandler<GetPendingAccountsQuery, GeneralResponse<GetDataResult<List<PendingAccountDTO>>>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetPendingAccountsQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;            
        }

        public async Task<GeneralResponse<GetDataResult<List<PendingAccountDTO>>>> Handle(GetPendingAccountsQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.Where(u=>!u.IsApproved)
                                        .OrderByDescending(u=>u.Startup!= null ?u.Startup.CreatedAt:u.ShippingCompany.CreatedAt)
                                        .Skip((request.PageNumber - 1) * request.PageSize)
                                        .Take(request.PageSize)
                                        .Select(u=>new PendingAccountDTO
                                        {
                                            Id= u.Startup != null ? u.Startup.Id : u.ShippingCompany.Id,
                                            UerId = u.Id,
                                            AccountType =u.Startup != null? "Startup": "Shipping Company",
                                            Email=u.Email,
                                            Name=u.Startup != null? u.Startup.CompanyName:u.ShippingCompany.CompanyName,
                                            ProfileImageUrl=u.ProfileImageUrl,
                                            RegisterAt = u.Startup != null ? u.Startup.CreatedAt : u.ShippingCompany.CreatedAt,
                                           
                                        }).ToListAsync(cancellationToken);

            if (!users.Any())
                return GeneralResponse<GetDataResult<List<PendingAccountDTO>>>.FailResponse("No pending accounts.");

            var dataResult = new GetDataResult<List<PendingAccountDTO>>
            {
                Data = users,
                TotalCount = await _userManager.Users.CountAsync(u=>!u.IsApproved),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
            return GeneralResponse<GetDataResult<List<PendingAccountDTO>>>.SuccessResponse("Pending accounts retreved successfully", dataResult);
        }
    }
}
