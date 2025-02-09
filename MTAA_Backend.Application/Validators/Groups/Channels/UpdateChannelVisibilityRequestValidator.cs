using FluentValidation;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using MTAA_Backend.Domain.Resources.Groups;

namespace MTAA_Backend.Application.Validators.Groups.Channels
{
    public class UpdateChannelVisibilityRequestValidator : AbstractValidator<UpdateChannelVisibilityRequest>
    {
        public UpdateChannelVisibilityRequestValidator()
        {
            var visibilities = GroupVisibilityTypes.GetAllPublic();

            RuleFor(x => x.Visibility)
                .Must(visibility => visibilities.Contains(visibility));
        }
    }
}
