using MediatR;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;

namespace MTAA_Backend.Application.CQRS.Users.Relationships.Queries
{
    public class GetFriends : IRequest<ICollection<PublicBaseAccountResponse>>
    {
        public PageParameters PageParameters { get; set; }
        public string? FilterStr { get; set; } = "";
    }
}
