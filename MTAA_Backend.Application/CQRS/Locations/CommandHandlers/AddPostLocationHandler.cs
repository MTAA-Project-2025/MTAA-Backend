using MediatR;
using MTAA_Backend.Application.CQRS.Locations.Commands;

namespace MTAA_Backend.Application.CQRS.Locations.CommandHandlers
{
    public class AddPostLocationHandler : IRequestHandler<AddPostLocation>
    {
        public async Task Handle(AddPostLocation request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
        }
    }
}
