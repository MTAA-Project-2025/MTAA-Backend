using FluentValidation;
using MTAA_Backend.Application.CQRS.Comments.Commands;
using MTAA_Backend.Domain.DTOs.Comments.Requests;

namespace MTAA_Backend.Application.Validators.Comments
{
    public class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
    {
        public AddCommentRequestValidator()
        {
            RuleFor(e => e.Content)
                .NotEmpty().WithMessage("Content cannot be empty.")
                .MaximumLength(2000).WithMessage("Content cannot be longer than 2000 characters.");
        }
    }
}
