using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs;
using Infrastructure.Database.Entities;
using Infrastructure.Providers.DateTime;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IDateTimeProvider _dateTime;
        private readonly IConfiguration _config;

        public TokenHelper(IDateTimeProvider dateTime, IConfiguration config)
        {
            _dateTime = dateTime;
            _config = config;
        }

        public string GenerateToken(UserDto user)
        {
            // Add standard claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email.Current),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, _dateTime.UtcNow().ToUnixTimeSeconds().ToString()), // Not Before
                new Claim(JwtRegisteredClaimNames.Exp,
                    _dateTime.UtcNow().AddDays(1).ToUnixTimeSeconds().ToString()), // Expires
            };

            // Add Roles
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

            
            // Create the token
            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("ReplaceFromConfigWhenAble")
                            ), SecurityAlgorithms.HmacSha256
                        )
                ),
                new JwtPayload(claims)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}