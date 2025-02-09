using FluentValidation;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;

namespace MTAA_Backend.Application.Validators.Account
{
    public class UpdateAccountBirthDateRequestValidator : AbstractValidator<UpdateAccountBirthDateRequest>
    {
        public UpdateAccountBirthDateRequestValidator()
        {
            this.RuleFor(e => e.BirthDate)
                .GreaterThan(new System.DateTime(1900, 1, 1))
                .WithMessage("Birth date must be greater than 1900-01-01")
                .LessThan(System.DateTime.UtcNow)
                .WithMessage("Birth date must be less than current date");
        }
    }
}
