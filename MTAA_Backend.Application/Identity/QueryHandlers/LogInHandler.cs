﻿using MediatR;
using MTAA_Backend.Application.Identity.Queries;
using MTAA_Backend.Domain.DTOs.Users.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Identity.QueryHandlers
{
    public class LogInHandler : IRequestHandler<LogIn, TokenDTO>
    {
        private readonly IStringLocalizer _localizer;
        private readonly UserManager<Customer> _userManager;
        private readonly IConfiguration _configuration;
        public LogInHandler(IStringLocalizer<ErrorMessages> localizer,
            UserManager<Customer> userManager,
            IConfiguration configuration)
        {
            _localizer = localizer;
            _userManager = userManager;
            _configuration = configuration;
        }
    }
}