using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Account.QueryHandlers
{
    public class PublicGetFullAccountHandler(ILogger<PublicGetFullAccountHandler> _logger,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext,
        IUserService _userService,
        IMapper _mapper) : IRequestHandler<PublicGetFullAccount, PublicFullAccountResponse>
    {
        public async Task<PublicFullAccountResponse> Handle(PublicGetFullAccount request, CancellationToken cancellationToken)
        {
            var customerId = _userService.GetCurrentUserId();

            var user = await _dbContext.Users.Where(e => e.Id == request.UserId)
                                             .Include(e => e.Avatar)
                                                 .ThenInclude(e => e.CustomAvatar)
                                                    .ThenInclude(e => e.Images)
                                             .Include(e => e.Avatar)
                                                 .ThenInclude(e => e.PresetAvatar)
                                                    .ThenInclude(e => e.Images)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                _logger.LogError($"User not found {request.UserId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserNotFound], HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<PublicFullAccountResponse>(user);

            if (customerId != null)
            {
                response.IsContact = await _dbContext.UserContacts.AnyAsync(e => e.UserId == customerId && e.ContactId == request.UserId, cancellationToken);
            }

            if (user.Avatar != null)
            {
                if (user.Avatar.CustomAvatar != null)
                {
                    response.Avatar = _mapper.Map<MyImageGroupResponse>(user.Avatar.CustomAvatar);
                }
                else if (user.Avatar.PresetAvatar != null)
                {
                    response.Avatar = _mapper.Map<MyImageGroupResponse>(user.Avatar.PresetAvatar);
                }
            }

            return response;
        }
    }
}
