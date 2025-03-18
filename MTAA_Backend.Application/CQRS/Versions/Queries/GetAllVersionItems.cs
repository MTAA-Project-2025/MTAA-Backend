using MediatR;
using MTAA_Backend.Domain.DTOs.Versioning.Responses;
using MTAA_Backend.Domain.Entities.Versions;

namespace MTAA_Backend.Application.CQRS.Versions.Queries
{
    public class GetAllVersionItems : IRequest<ICollection<VersionItemResponse>>
    {
    }
}
