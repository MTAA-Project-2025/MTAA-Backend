using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MTAA_Backend.Application.CQRS.Users.Identity.Commands;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Users.Identity.Other;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Customers;
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
    public class SignUpVerifyEmailHandler(IDistributedCache _distributedCache,
        IStringLocalizer<ErrorMessages> _localizer,
        MTAA_BackendDbContext _dbContext) : IRequestHandler<SignUpVerifyEmail, bool>
    {
        public async Task<bool> Handle(SignUpVerifyEmail request, CancellationToken cancellationToken)
        {
            var oldUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (oldUser != null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.EmailAlreadyExists], HttpStatusCode.BadRequest);
            }

            var recordId = "EmailVerificationCode_" + request.Email;
            var codeModel = await _distributedCache.GetRecordAsync<VerificationCodeModel>(recordId);
            if (codeModel == null) return false;

            if (DateTime.UtcNow > codeModel.ExpirationTime)
            {
                await _distributedCache.RemoveAsync(recordId, cancellationToken);
                return false;
            }
            if (codeModel.Code != request.Code)
            {
                return false;
            }
            codeModel.IsVerified = true;
            await _distributedCache.SetRecordAsync(recordId, codeModel);
            return true;
        }
    }
}
