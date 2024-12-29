using FluentValidation;
using MTAA_Backend.Domain.DTOs.Users.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Validators.Identity
{
    public class LogInRequestValidator : AbstractValidator<LogInRequest>
    {
        public LogInRequestValidator()
        {
            RuleFor(e => e.Email)
                .MaximumLength(200);

            RuleFor(e => e.Password)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(e => e.PhoneNumber)
                .MaximumLength(20);
        }
    }
}
