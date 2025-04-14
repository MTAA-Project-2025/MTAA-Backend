using FluentValidation;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;

namespace MTAA_Backend.Application.Validators.Account
{
    public class CustomUpdateAccountAvatarRequestValidator : AbstractValidator<CustomUpdateAccountAvatarRequest>
    {
        public CustomUpdateAccountAvatarRequestValidator()
        {
            this.RuleFor(e => e.Avatar)
                .NotNull()
                .ChildRules(p => p.RuleFor(e => e.Length)
                    .GreaterThan(0)
                    .WithMessage("The image should be not empty")
                    .LessThanOrEqualTo(10 * 1024 * 1024)
                    .WithMessage("The image should not be bigger than 10 MB"));
        }
    }
}
