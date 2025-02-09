using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Application.Identity.Commands;
using MTAA_Backend.Domain.DTOs.Users.Identity.Other;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Domain.Resources.Other;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Identity.CommandHandlers
{
    public class StartSignUpEmailVerificationHandler(IDistributedCache distributedCache,
        IEmailService emailService,
        ICodeGeneratorService codeGeneratorService,
        IStringLocalizer<ErrorMessages> localizer,
        MTAA_BackendDbContext dbContext) : IRequestHandler<StartSignUpEmailVerification>
    {
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly IEmailService _emailService = emailService;
        private readonly ICodeGeneratorService _codeGeneratorService = codeGeneratorService;
        private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;
        private readonly MTAA_BackendDbContext _dbContext = dbContext;

        public async Task Handle(StartSignUpEmailVerification request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user != null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.EmailAlreadyExists], HttpStatusCode.BadRequest);
            }

            var recordId = "EmailVerificationCode_" + request.Email;
            var codeModel = await _distributedCache.GetRecordAsync<VerificationCodeModel>(recordId);
            if (codeModel != null)
            {
                if (DateTime.UtcNow < codeModel.ExpirationTime)
                {
                    throw new HttpException(_localizer[ErrorMessagesPatterns.VerificationCodeAlreadySent], HttpStatusCode.BadRequest);
                }
            }
            string code = _codeGeneratorService.Generate6DigitCode();
            codeModel = new VerificationCodeModel()
            {
                Code = code,
                ExpirationTime = DateTime.UtcNow.AddMinutes(TimeConstants.VerificationSpan.TotalMinutes),
                IsVerified = false
            };

            await _distributedCache.SetRecordAsync(recordId, codeModel);
            await _emailService.SendSighUpVerificationEmail(request.Email, code);
        }
    }
}
