using FluentValidation;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;

namespace MTAA_Backend.Application.Validators.Groups.Channels
{
    public class UpdateChannelIdentificationNameRequestValidator : AbstractValidator<UpdateChannelIdentificationNameRequest>
    {
        public UpdateChannelIdentificationNameRequestValidator()
        {
            RuleFor(e => e.IdentificationName)
                .MinimumLength(3).WithMessage("Identification name must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Identification name must be at most 50 characters long.")
                .AllowedCharactersOnly("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_");
        }
    }
}
