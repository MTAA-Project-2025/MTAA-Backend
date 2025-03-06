using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.CQRS.Groups.Channels.Commands;
using MTAA_Backend.Application.CQRS.Locations.Commands;
using MTAA_Backend.Application.CQRS.Posts.Commands;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Posts.Requests;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System.Net;

namespace MTAA_Backend.Api.Controllers.Posts
{
    public class PostsController : ApiController
    {
        private readonly MTAA_BackendDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IUserService _userService;
        public PostsController(IMediator mediator,
            IMapper mapper,
            MTAA_BackendDbContext dbContext,
            IStringLocalizer<ErrorMessages> localizer,
            IUserService userService) : base(mediator, mapper)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.User)]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> AddPost([FromForm] AddPostRequest request)
        {
            var command = _mapper.Map<AddPost>(request);
            var res = await _mediator.Send(command);
            await _mediator.Send(new AddPostLocation()
            {
                PostId = res
            });
            return Ok(res);
        }
    }
}
