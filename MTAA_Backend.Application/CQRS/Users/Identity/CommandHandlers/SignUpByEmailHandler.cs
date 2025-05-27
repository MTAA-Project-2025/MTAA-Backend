using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.Events;
using MTAA_Backend.Application.CQRS.Users.Identity.Queries;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Users.Identity.Other;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Identity.CommandHandlers
{
    /// <summary>
    /// Handles the <see cref="SignUpByEmail"/> command to register a new user account using email and password.
    /// It verifies email verification status and assigns the 'User' role upon successful registration.
    /// </summary>
    public class SignUpByEmailHandler(IDistributedCache _distributedCache,
        ILogger<SignUpByEmailHandler> _logger,
        UserManager<User> _userManager,
        RoleManager<IdentityRole> _roleManager,
        IStringLocalizer<ErrorMessages> _localizer,
        IMediator _mediator,
        MTAA_BackendDbContext _dbContext) : IRequestHandler<SignUpByEmail, TokenDTO>
    {
        /// <summary>
        /// Handles the <see cref="SignUpByEmail"/> command.
        /// </summary>
        /// <param name="request">The <see cref="SignUpByEmail"/> command request, containing email, username, and password.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A <see cref="TokenDTO"/> containing authentication tokens upon successful registration and login.</returns>
        /// <exception cref="HttpException">Thrown if the email already exists, the signup session has expired, the email is not verified, or user creation fails.</exception>
        public async Task<TokenDTO> Handle(SignUpByEmail request, CancellationToken cancellationToken)
        {
            var oldUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (oldUser != null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.EmailAlreadyExists], HttpStatusCode.BadRequest);
            }

            var recordId = "EmailVerificationCode_" + request.Email;
            var codeModel = await _distributedCache.GetRecordAsync<VerificationCodeModel>(recordId);
            if (codeModel == null)
            {
                _logger.LogError($"Error while getting verification code from cache: {recordId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.SignUpSessionExpired], HttpStatusCode.BadRequest);
            }
            if (!codeModel.IsVerified)
            {
                _logger.LogError($"Creating account while not verified {recordId}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.EmailIsNotVerified], HttpStatusCode.BadRequest);
            }

            var user = new User()
            {
                Email = request.Email,
                UserName = request.UserName,
                DisplayName = request.UserName,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                _logger.LogError($"Error while creating user: {result.Errors}");
                throw new HttpException(_localizer[ErrorMessagesPatterns.UserCreationError], HttpStatusCode.BadRequest);
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            else
            {
                _logger.LogError($"Error while creating role: {result.Errors}");
            }

            await _mediator.Publish(new CreateAccountEvent()
            {
                UserId = user.Id
            });

            return await _mediator.Send(new LogIn()
            {
                Email = request.Email,
                Password = request.Password
            }, cancellationToken);
        }
    }
}
