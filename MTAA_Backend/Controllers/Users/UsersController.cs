using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Application.CQRS.Users.Relationships.Commands;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Resources.Customers;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Users
{
    public class UsersController : ApiController
    {
        public UsersController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        [Route("public-full-account/{userId}")]
        [ProducesResponseType(typeof(PublicFullAccountResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PublicFullAccountResponse>> PublicGetFullAccount([FromRoute] string userId)
        {
            var query = new PublicGetFullAccount()
            {
                UserId = userId
            };
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-global")]
        [ProducesResponseType(typeof(ICollection<PublicBaseAccountResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<PublicBaseAccountResponse>>> GetGlobalUsers([FromBody] GlobalSearchRequest request)
        {
            var command = _mapper.Map<GetGlobalUsers>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        [HttpPost("follow")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Follow([FromBody] Follow request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("unfollow")]
        [Authorize(Roles = UserRoles.User)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Unfollow([FromBody] Unfollow request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
