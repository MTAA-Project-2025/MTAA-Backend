using FluentValidation;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;

namespace MTAA_Backend.Application.Validators.Account
{
    public class UpdateAccountDisplayNameRequestValidator : AbstractValidator<UpdateAccountDisplayNameRequest>
    {
        public UpdateAccountDisplayNameRequestValidator()
        {
            this.RuleFor(e => e.DisplayName)
                .AbstractName();
        }
    }
}
