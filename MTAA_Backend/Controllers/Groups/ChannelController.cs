using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Groups
{
    public class ChannelController : ApiController
    {
        public ChannelController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> AddChannel([FromForm] AddChannelRequest request)
        {
            var command = _mapper.Map<AddChannel>(request);
            var res = await _mediator.Send(command);
            return Ok(res);
        }
    }
}
