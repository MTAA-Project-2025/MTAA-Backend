﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Events;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Account.CommandHandlers
{
    public class UpdateAccountBirthDateHandler(MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMediator _mediator) : IRequestHandler<UpdateAccountBirthDate>
    {
        public async Task Handle(UpdateAccountBirthDate request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUser();
            user.BirthDate = request.BirthDate.ToUniversalTime();
            user.IsEdited = true;
            user.DataLastEditTime = DateTime.UtcNow;

            await _mediator.Publish(new AccountUpdateEvent()
            {
                UserId = user.Id
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
