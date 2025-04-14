using MTAA_Backend.Application.CQRS.Posts.Queries;
using MTAA_Backend.Application.CQRS.Users.Account.Commands;
using MTAA_Backend.Application.CQRS.Users.Account.Queries;
using MTAA_Backend.Application.CQRS.Users.Identity.Commands;
using MTAA_Backend.Application.CQRS.Users.Identity.Queries;
using MTAA_Backend.Domain.DTOs.Shared.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using MTAA_Backend.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.MaperProfiles.Users
{
    public class UserMapperProfile : AutoMapper.Profile
    {
        public UserMapperProfile()
        {
            CreateMap<LogInRequest, LogIn>();
            CreateMap<StartSignUpEmailVerificationRequest, StartSignUpEmailVerification>();
            CreateMap<SignUpByEmailRequest, SignUpByEmail>();
            CreateMap<SignUpVerifyEmailRequest, SignUpVerifyEmail>();

            CreateMap<CustomUpdateAccountAvatarRequest, CustomUpdateAccountAvatar>();
            CreateMap<PresetUpdateAccountAvatarRequest, PresetUpdateAccountAvatar>();
            CreateMap<UpdateAccountBirthDateRequest, UpdateAccountBirthDate>();
            CreateMap<UpdateAccountDisplayNameRequest, UpdateAccountDisplayName>();
            CreateMap<UpdateAccountUsernameRequest, UpdateAccountUsername>();

            CreateMap<User, PublicFullAccountResponse>()
                .ForMember(dest => dest.Avatar,
                           opt => opt.Ignore())
                .ForMember(dest => dest.IsFollowing,
                           opt => opt.Ignore())
                .ForMember(dest => dest.FriendsCount,
                           opt => opt.Ignore())
                .ForMember(dest => dest.FollowersCount,
                           opt => opt.Ignore());

            CreateMap<User, UserFullAccountResponse>()
                .ForMember(dest => dest.Avatar,
                           opt => opt.Ignore())
                .ForMember(dest => dest.IsFollowing,
                           opt => opt.Ignore())
                .ForMember(dest => dest.FriendsCount,
                           opt => opt.Ignore())
                .ForMember(dest => dest.FollowersCount,
                           opt => opt.Ignore())
                .ForMember(dest => dest.LikesCount,
                           opt => opt.Ignore());

            CreateMap<User, PublicBaseAccountResponse>()
                .ForMember(dest => dest.Avatar,
                           opt => opt.Ignore())
                .ForMember(dest => dest.IsFollowing,
                           opt => opt.Ignore());

            CreateMap<GlobalSearchRequest, GetGlobalUsers>();
        }
    }
}
