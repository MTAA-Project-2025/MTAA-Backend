using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Application.CQRS.Locations.Queries;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Domain.DTOs.Locations.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Locations
{
    public class LocationsController : ApiController
    {
        public LocationsController(IMediator mediator,
            IMapper mapper) : base(mediator, mapper)
        {
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-points")]
        [ProducesResponseType(typeof(ICollection<GetLocationPoints>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<GetLocationPoints>>> GetLocationPoints([FromBody] GetLocationPointsRequest request)
        {
            var command = _mapper.Map<GetLocationPoints>(request);
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-cluster-posts/{clusterId}")]
        [ProducesResponseType(typeof(ICollection<LocationPostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<LocationPostResponse>>> AddPost([FromRoute] Guid clusterId, [FromBody] PageParameters pageParameters)
        {
            var res = await _mediator.Send(new GetClusterLocationPosts()
            {
                CluserPointId = clusterId,
                PageParameters = pageParameters
            });

            return Ok(res);
        }
    }
}
