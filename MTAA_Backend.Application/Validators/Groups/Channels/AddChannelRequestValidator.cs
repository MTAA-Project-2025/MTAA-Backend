using FluentValidation;
using MTAA_Backend.Application.Extensions;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.Resources.Groups;

namespace MTAA_Backend.Application.Validators.Groups.Channels
{
    public class AddChannelRequestValidator : AbstractValidator<AddChannelRequest>
    {
        public AddChannelRequestValidator()
        {
            var visibilities = GroupVisibilityTypes.GetAllPublic();

            RuleFor(e => e.IdentificationName)
                .MinimumLength(3).WithMessage("Identification name must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Identification name must be at most 50 characters long.")
                .AllowedCharactersOnly("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_");

            RuleFor(x => x.DisplayName)
                .AbstractName();

            RuleFor(x => x.Description)
                .MaximumLength(3000);

            RuleFor(x => x.Visibility)
                .Must(visibility => visibilities.Contains(visibility));

            this.RuleFor(e => e.Image)
                .ChildRules(p => p.RuleFor(e => e.Length)
                    .GreaterThan(0)
                    .WithMessage("The image should be not empty")
                    .LessThanOrEqualTo(10 * 1024 * 1024)
                    .WithMessage("The image should be bigger than 10 MB"));
        }
    }
}
