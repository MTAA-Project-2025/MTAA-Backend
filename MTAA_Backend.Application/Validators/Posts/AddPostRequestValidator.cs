using FluentValidation;
using MTAA_Backend.Domain.DTOs.Posts.Requests;

namespace MTAA_Backend.Application.Validators.Posts
{
    public class AddPostRequestValidator : AbstractValidator<AddPostRequest>
    {
        public AddPostRequestValidator()
        {
            this.RuleFor(e => e.Description)
                .MinimumLength(3)
                .MaximumLength(3000);

            this.RuleFor(e => e.Images)
                .NotEmpty();

            this.RuleForEach(e => e.Images)
                .NotNull()
                .ChildRules(p => p.RuleFor(e => e.Length)
                    .GreaterThan(0)
                    .WithMessage("The image should be not empty")
                    .LessThanOrEqualTo(10 * 1024 * 1024)
                    .WithMessage("The image should not be bigger than 10 MB"));
        }
    }
}
