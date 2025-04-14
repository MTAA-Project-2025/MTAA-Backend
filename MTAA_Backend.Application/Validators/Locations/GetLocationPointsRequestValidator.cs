using FluentValidation;
using MTAA_Backend.Domain.DTOs.Locations.Requests;
using MTAA_Backend.Domain.Resources.Locations;

namespace MTAA_Backend.Application.Validators.Locations
{
    public class GetLocationPointsRequestValidator : AbstractValidator<GetLocationPointsRequest>
    {
        public GetLocationPointsRequestValidator()
        {
            RuleFor(x => x.Radius)
                .GreaterThan(0)
                .WithMessage("Radius must be greater than 0")
                .LessThanOrEqualTo(LocationConstants.MAX_RADIUS)
                .WithMessage($"Radius must be less than equator");

            RuleFor(x => x.Latitude)
                .GreaterThanOrEqualTo(LocationConstants.MIN_LATITUDE)
                .LessThanOrEqualTo(LocationConstants.MAX_LATITUDE);

            RuleFor(x => x.Longitude)
                .GreaterThanOrEqualTo(LocationConstants.MIN_LONGITUDE)
                .LessThanOrEqualTo(LocationConstants.MAX_LONGITUDE);

            RuleFor(x => x.ZoomLevel)
                .GreaterThanOrEqualTo(LocationConstants.MIN_ZOOM_LEVEL)
                .LessThanOrEqualTo(LocationConstants.MAX_ZOOM_LEVEL);
        }
    }
}
