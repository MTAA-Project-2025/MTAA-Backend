using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Application.CQRS.Locations.Queries;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Domain.DTOs.Locations.Requests;
using MTAA_Backend.Domain.DTOs.Locations.Responses;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Responses;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Locations
{
    /// <summary>
    /// Controller for managing location-related operations, including retrieving location points and posts.
    /// </summary>
    public class LocationsController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        public LocationsController(IMediator mediator,
            IMapper mapper) : base(mediator, mapper)
        {
        }

        /// <summary>
        /// Retrieves a collection of location points based on the provided criteria.
        /// </summary>
        /// <param name="request">The request containing criteria for filtering location points (e.g., geographic bounds).</param>
        /// <returns>A collection of location points.</returns>
        /// <response code="200">Returns the list of location points.</response>
        /// <response code="400">If the request is invalid or malformed.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-points")]
        [ProducesResponseType(typeof(ICollection<SimpleLocationPointResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<SimpleLocationPointResponse>>> GetLocationPoints([FromBody] GetLocationPointsRequest request)
        {
            var command = _mapper.Map<GetLocationPoints>(request);
            var res = await _mediator.Send(command);

            return Ok(res);
        }

        /// <summary>
        /// Retrieves paginated posts associated with a specific location cluster.
        /// </summary>
        /// <param name="clusterId">The GUID of the location cluster to retrieve posts for.</param>
        /// <param name="pageParameters">Pagination parameters (e.g., page number, page size).</param>
        /// <returns>A collection of posts associated with the cluster.</returns>
        /// <response code="200">Returns the list of posts.</response>
        /// <response code="400">If the cluster ID is not a valid GUID or pagination parameters are invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the cluster does not exist.</response>
        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-cluster-posts/{clusterId}")]
        [ProducesResponseType(typeof(ICollection<LocationPostResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<LocationPostResponse>>> GetClusterLocationPosts([FromRoute] string clusterId, [FromBody] PageParameters pageParameters)
        {
            Guid parsedId;
            var parseRes = Guid.TryParse(clusterId, out parsedId);
            if (!parseRes)
            {
                throw new HttpException("ClusterId is not in the correct format of GUID", HttpStatusCode.BadRequest);
            }
            var res = await _mediator.Send(new GetClusterLocationPosts()
            {
                CluserPointId = parsedId,
                PageParameters = pageParameters
            });

            return Ok(res);
        }

        /// <summary>
        /// Retrieves a single location post by its ID.
        /// </summary>
        /// <param name="id">The GUID of the location post to retrieve.</param>
        /// <returns>The details of the location post.</returns>
        /// <response code="200">Returns the location post details.</response>
        /// <response code="400">If the post ID is not a valid GUID.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user lacks the required role.</response>
        /// <response code="404">If the post does not exist.</response>
        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        [Route("get-location-post/{id}")]
        [ProducesResponseType(typeof(LocationPostResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LocationPostResponse>> GetClusterLocationPosts([FromRoute] string id)
        {
            Guid parsedId;
            var parseRes = Guid.TryParse(id, out parsedId);
            if (!parseRes)
            {
                throw new HttpException("Id is not in the correct format of GUID", HttpStatusCode.BadRequest);
            }
            var res = await _mediator.Send(new GetLocationPostById()
            {
                Id = parsedId
            });

            return Ok(res);
        }
    }
}
