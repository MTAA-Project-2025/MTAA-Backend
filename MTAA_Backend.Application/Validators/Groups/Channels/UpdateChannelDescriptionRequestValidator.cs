using FluentValidation;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;

namespace MTAA_Backend.Application.Validators.Groups.Channels
{
    public class UpdateChannelDescriptionRequestValidator : AbstractValidator<UpdateChannelDescriptionRequest>
    {
        public UpdateChannelDescriptionRequestValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(3000);
        }
    }
}
