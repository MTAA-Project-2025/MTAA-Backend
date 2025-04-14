using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MTAA_Backend.Application.CQRS.Users.Identity.Queries;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Resources.Localization.Errors;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.CQRS.Users.Identity.QueryHandlers
{
    public class LogInHandler(IStringLocalizer<ErrorMessages> _localizer,
            UserManager<User> _userManager,
            IConfiguration _configuration,
            MTAA_BackendDbContext _dbContext,
            ILogger<LogInHandler> _logger) : IRequestHandler<LogIn, TokenDTO>
    {
        public async Task<TokenDTO> Handle(LogIn request, CancellationToken cancellationToken)
        {
            if (request.PhoneNumber == null && request.Email == null)
            {
                _logger.LogError("While loading, Email and Phone Number are null");
                throw new HttpException(_localizer[ErrorMessagesPatterns.EmailAndPhoneNumberNull], HttpStatusCode.BadRequest);
            }

            User? user;
            if (request.Email != null)
            {
                user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) throw new HttpException(_localizer[ErrorMessagesPatterns.UserBadEmail], HttpStatusCode.NotFound);
            }
            else
            {
                user = await _dbContext.Users.Where(e => e.PhoneNumber == request.PhoneNumber).FirstOrDefaultAsync(cancellationToken);
                if (user == null) throw new HttpException(_localizer[ErrorMessagesPatterns.UserBadPhoneNumber], HttpStatusCode.NotFound);
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password)) throw new HttpException(_localizer[ErrorMessagesPatterns.UserBadPassword], HttpStatusCode.BadRequest);

            return new TokenDTO()
            {
                Token = await CreateTokenAsync(user)
            };
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("JwtOptions");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Id", user.Id)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtOptions");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(Convert.ToDouble(jwtSettings["Lifetime"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}
