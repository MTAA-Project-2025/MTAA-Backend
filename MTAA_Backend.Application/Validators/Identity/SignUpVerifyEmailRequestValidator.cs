using FluentValidation;
using MTAA_Backend.Domain.DTOs.Users.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Validators.Identity
{
    public class SignUpVerifyEmailRequestValidator : AbstractValidator<SignUpVerifyEmailRequest>
    {
        public SignUpVerifyEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(200);

            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(6);
        }
    }
}
