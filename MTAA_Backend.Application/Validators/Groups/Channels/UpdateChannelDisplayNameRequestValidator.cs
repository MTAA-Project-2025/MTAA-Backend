using FluentValidation;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;

namespace MTAA_Backend.Application.Validators.Groups.Channels
{
    public class UpdateChannelDisplayNameRequestValidator : AbstractValidator<UpdateChannelDisplayNameRequest>
    {
        public UpdateChannelDisplayNameRequestValidator()
        {
            RuleFor(x => x.DisplayName)
                .AbstractName();
        }
    }
}
