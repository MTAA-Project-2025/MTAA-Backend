﻿using FluentValidation;
using MTAA_Backend.Domain.DTOs.Users.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Validators.Identity
{
    public class SignUpByEmailRequestValidator : AbstractValidator<SignUpByEmailRequest>
    {
        public SignUpByEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(200);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(200);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}