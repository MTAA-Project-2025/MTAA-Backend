using MediatR;
using MTAA_Backend.Domain.DTOs.Images.Response;

namespace MTAA_Backend.Application.CQRS.Images.PresetImages.Queries
{
    public class GetAllPresetImages : IRequest<ICollection<MyImageGroupResponse>>
    {
    }
}
