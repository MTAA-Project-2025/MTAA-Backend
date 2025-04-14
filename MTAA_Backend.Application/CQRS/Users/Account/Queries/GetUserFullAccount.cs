using MediatR;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;

namespace MTAA_Backend.Application.CQRS.Users.Account.Queries
{
    public class GetUserFullAccount : IRequest<UserFullAccountResponse>
    {
    }
}
