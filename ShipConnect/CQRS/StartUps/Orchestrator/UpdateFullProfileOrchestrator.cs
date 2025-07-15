
using ShipConnect.CQRS.StartUps.Commands;
using ShipConnect.CQRS.UserCQRS.Commands;

namespace ShipConnect.CQRS.StartUps.Orchestrator
{
    public class UpdateFullProfileOrchestrator : IRequest< bool>
    {
        public string Id { get; set; }
        public UpdateFullProfileDTO DTO { get; set; }
        
    }
    public class UpdateFullProfileOrchestratorHandler : IRequestHandler<UpdateFullProfileOrchestrator, bool>
    {
        private readonly IMediator _mediator;
        public UpdateFullProfileOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<bool> Handle(UpdateFullProfileOrchestrator request, CancellationToken cancellationToken)
        {
            var userResult = await _mediator.Send(new EditUserCommand(request.Id, request.DTO.UserDTO), cancellationToken);

            bool startupOk = false;
            if (userResult.Succeeded)
            {
                startupOk = await _mediator.Send(new EditStartupCommand
                {
                    Id = request.Id,
                    Data = request.DTO.StartupDTO
                });
            }

            return startupOk && userResult.Succeeded; 
        }
    }
}
