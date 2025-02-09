using FluentValidation;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;

namespace MTAA_Backend.Application.Validators.Account
{
    public class UpdateAccountUsernameRequestValidator : AbstractValidator<UpdateAccountUsernameRequest>
    {
        public UpdateAccountUsernameRequestValidator()
        {
            this.RuleFor(e => e.Username)
                .Username();
        }
    }
}
