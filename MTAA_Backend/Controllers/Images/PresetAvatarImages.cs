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
    public class PresetAvatarImages : ApiController
    {
        public PresetAvatarImages(IMediator mediator,
            IMapper mapper) : base(mediator, mapper)
        {
        }

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
