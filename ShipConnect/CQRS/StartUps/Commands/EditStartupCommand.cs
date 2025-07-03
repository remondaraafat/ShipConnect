

using ShipConnect.DTOs.StartUpDTOs;

namespace ShipConnect.CQRS.StartUps.Commands
{
    public class EditStartupCommand:IRequest
    {
        public EditStartupDTO DTO { get; set; }
    }
    public class EditStartupCommandHandler : IRequestHandler<EditStartupCommand>
    {
        public Task Handle(EditStartupCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
