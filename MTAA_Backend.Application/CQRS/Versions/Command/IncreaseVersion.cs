using MediatR;
using MTAA_Backend.Domain.Entities.Versions;

namespace MTAA_Backend.Application.CQRS.Versions.Command
{
    public class IncreaseVersion : IRequest
    {
        public string UserId { get; set; }
        public VersionItemType VersionItemType { get; set; }
    }
}
