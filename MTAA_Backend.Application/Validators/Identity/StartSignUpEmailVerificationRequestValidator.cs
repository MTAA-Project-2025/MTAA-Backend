using FluentValidation;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Validators.Identity
{
    public class StartSignUpEmailVerificationRequestValidator : AbstractValidator<StartSignUpEmailVerificationRequest>
    {
        public StartSignUpEmailVerificationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(200);
        }
    }
}
