using MTAA_Backend.Application.Account.Commands;
using MTAA_Backend.Application.Identity.Commands;
using MTAA_Backend.Application.Identity.Queries;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.MaperProfiles.User
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
        }
    }
}
