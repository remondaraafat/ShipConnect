
using ShipConnect.CQRS.StartUps.Commands;
using ShipConnect.CQRS.UserCQRS.Commands;

namespace ShipConnect.CQRS.StartUps.Orchestrator
{
    public class UpdateFullProfileOrchestrator : IRequest< bool>
    {
        public string Email { get; set; }
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
            var userResult = await _mediator.Send(new EditUserCommand
            {
                Email = request.Email,
                DTO = request.DTO.UserDTO
            });

            bool startupOk = false;
            if (userResult.Succeeded)
            {
                startupOk = await _mediator.Send(new EditStartupCommand
                {
                    Email = request.Email,
                    Data = request.DTO.StartupDTO
                });
            }

            return startupOk && userResult.Succeeded; 
        }
    }
}
