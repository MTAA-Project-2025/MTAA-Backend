using MediatR;
using MTAA_Backend.Application.Identity.Queries;
using MTAA_Backend.Domain.DTOs.Users.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Identity.QueryHandlers
{
    public class LogInHandler : IRequestHandler<LogIn, TokenDTO>
    {
        private readonly IStringLocalizer _localizer;
        private readonly UserManager<Customer> _userManager;
        private readonly IConfiguration _configuration;
        public LogInHandler(IStringLocalizer<ErrorMessages> localizer,
            UserManager<Customer> userManager,
            IConfiguration configuration)
        {
            _localizer = localizer;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<TokenDTO> Handle(LogIn request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new HttpException(_localizer[ErrorMessagesPatterns.UserBadEmail], HttpStatusCode.NotFound);

            if (!await _userManager.CheckPasswordAsync(user, request.Password)) throw new HttpException(_localizer[ErrorMessagesPatterns.UserBadPassword], HttpStatusCode.BadRequest);

            return new TokenDTO()
            {
                Token = await CreateTokenAsync(user)
            };
        }

        private async Task<string> CreateTokenAsync(Customer user)
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
        private async Task<List<Claim>> GetClaims(Customer user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
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
