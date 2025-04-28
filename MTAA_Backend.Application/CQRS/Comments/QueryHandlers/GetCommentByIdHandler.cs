using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using MTAA_Backend.Application.CQRS.Comments.Queries;
using MTAA_Backend.Domain.DTOs.Comments.Responses;
using MTAA_Backend.Domain.DTOs.Images.Response;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Comments;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;

namespace MTAA_Backend.Application.CQRS.Comments.QueryHandlers
{
    public class GetCommentByIdHandler(MTAA_BackendDbContext _dbContext, IMapper _mapper,
        IUserService _userService,
        IStringLocalizer<ErrorMessages> _localizer)
    : IRequestHandler<GetCommentById, FullCommentResponse>
    {
        public async Task<FullCommentResponse> Handle(GetCommentById request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            var item = await _dbContext.Comments
                .Where(e => e.Id == request.Id)
                .Include(e => e.Owner)
                    .ThenInclude(e => e.Avatar)
                        .ThenInclude(e => e.CustomAvatar)
                            .ThenInclude(e => e.Images)
                .Include(e => e.Owner)
                    .ThenInclude(e => e.Avatar)
                        .ThenInclude(e => e.PresetAvatar)
                            .ThenInclude(e => e.Images)
                .Select(c => new
                {
                    Comment = c,
                    InteractionType = c.CommentInteractions
                    .Where(ui => ui.UserId == userId)
                    .Select(ui => (CommentInteractionType?)ui.Type)
                    .FirstOrDefault() ?? CommentInteractionType.None
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
            {
                throw new HttpException(_localizer[ErrorMessagesPatterns.CommentNotFound], System.Net.HttpStatusCode.NotFound);
            }

            FullCommentResponse res;


            res = _mapper.Map<FullCommentResponse>(item.Comment);
            res.Type = item.InteractionType;

            var ownerAvatar = item.Comment.Owner.Avatar;
            if (ownerAvatar != null)
            {
                if (ownerAvatar.CustomAvatar != null)
                {
                    res.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(ownerAvatar.CustomAvatar);
                }
                else if (ownerAvatar.PresetAvatar != null)
                {
                    res.Owner.Avatar = _mapper.Map<MyImageGroupResponse>(ownerAvatar.PresetAvatar);
                }
            }

            return res;
        }
    }
}
