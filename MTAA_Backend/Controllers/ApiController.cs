using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MTAA_Backend.Api.Controllers
{
    /// <summary>
    /// Base controller for all API controllers, providing shared dependencies and API versioning.
    /// </summary>
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// The MediatR mediator for handling commands and queries.
        /// </summary>
        protected readonly IMediator _mediator;

        /// <summary>
        /// The AutoMapper instance for mapping DTOs to commands or queries.
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR mediator for handling commands and queries.</param>
        /// <param name="mapper">The AutoMapper instance for mapping DTOs to commands or queries.</param>
        public ApiController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
    }
}
