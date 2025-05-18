using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Images.PresetImages.Queries;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Images
{
    /// <summary>
    /// Controller for retrieving preset avatar images.
    /// </summary>
    public class PresetAvatarImagesController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresetAvatarImagesController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands.</param>
        public PresetAvatarImagesController(IMediator mediator,
            IMapper mapper) : base(mediator, mapper)
        {
        }

        /// <summary>
        /// Retrieves all preset avatar images available in the system.
        /// </summary>
        /// <returns>A collection of preset avatar image details.</returns>
        /// <response code="200">Returns the list of preset avatar images.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(ICollection<MyImageGroupResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ICollection<MyImageGroupResponse>>> GetAllPresetImages()
        {
            var command = new GetAllPresetImages();
            var res = await _mediator.Send(command);
            return Ok(res);
        }
    }
}
