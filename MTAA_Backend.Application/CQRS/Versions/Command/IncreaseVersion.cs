using MediatR;
using MTAA_Backend.Domain.Resources.Versioning;

namespace MTAA_Backend.Application.CQRS.Versions.Command
{
    public class IncreaseVersion : IRequest
    {
        public string UserId { get; set; }
        public VersionItemType VersionItemType { get; set; }
    }
}
