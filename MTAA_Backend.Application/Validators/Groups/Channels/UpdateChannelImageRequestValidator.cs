using FluentValidation;
using MTAA_Backend.Domain.DTOs.Groups.Channels.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;

namespace MTAA_Backend.Application.Validators.Groups.Channels
{
    public class UpdateChannelImageRequestValidator : AbstractValidator<UpdateChannelImageRequest>
    {
        public UpdateChannelImageRequestValidator()
        {
            this.RuleFor(e => e.Image)
                    .ChildRules(p => p.RuleFor(e => e.Length)
                    .GreaterThan(0)
                    .WithMessage("The image should be not empty")
                    .LessThanOrEqualTo(10 * 1024 * 1024)
                    .WithMessage("The image should be bigger than 10 MB"));
        }
    }
}
