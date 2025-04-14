using MediatR;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;

namespace MTAA_Backend.Application.CQRS.Users.Account.Queries
{
    public class GetGlobalUsers : IRequest<ICollection<PublicBaseAccountResponse>>
    {
        public string? FilterStr { get; set; } = "";
        public PageParameters PageParameters { get; set; }
    }
}
