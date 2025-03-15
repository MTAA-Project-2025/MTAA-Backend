using MediatR;
using MTAA_Backend.Application.CQRS.Locations.Commands;

namespace MTAA_Backend.Application.CQRS.Locations.CommandHandlers
{
    public class UpdatePostLocationHandler : IRequestHandler<UpdatePostLocation>
    {
        public async Task Handle(UpdatePostLocation request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
        }
    }
}
