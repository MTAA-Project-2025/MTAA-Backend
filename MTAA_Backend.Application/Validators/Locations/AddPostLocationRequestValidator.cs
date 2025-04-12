using FluentValidation;
using MTAA_Backend.Domain.DTOs.Locations.Requests;
using MTAA_Backend.Domain.Resources.Locations;

namespace MTAA_Backend.Application.Validators.Locations
{
    public class AddPostLocationRequestValidator : AbstractValidator<AddLocationRequest>
    {
        public AddPostLocationRequestValidator()
        {
            RuleFor(e => e.EventTime)
                .GreaterThan(DateTime.UtcNow);

            RuleFor(e => e.Latitude)
                .GreaterThanOrEqualTo(LocationConstants.MIN_LATITUDE)
                .LessThanOrEqualTo(LocationConstants.MAX_LATITUDE);

            RuleFor(e => e.Longitude)
                .GreaterThanOrEqualTo(LocationConstants.MIN_LONGITUDE)
                .LessThanOrEqualTo(LocationConstants.MAX_LONGITUDE);
        }
    }
}
