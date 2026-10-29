using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PostsTags.Core.Entities;
using PostsTags.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PostsTags.Infrastructure.Authentication
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<(string Token, DateTime ExpiresAtUtc)> CreateTokenAsync(AppUser user)
        {
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT signing key is not configured.");
            var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
            var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience is not configured.");

            var expiresAtUtc = DateTime.UtcNow.AddHours(1); // Token expiration time

            if(expiresAtUtc < DateTime.UtcNow)
            {
                throw new InvalidOperationException("JWT expiration must be in the future.");
            }
            var keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length < 32)
            {
                throw new InvalidOperationException(
                    "JWT signing key must be at least 32 bytes.");
            }
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(ClaimTypes.Name, user.Name),
            new(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

            var securityKey = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler()
                .WriteToken(jwt);

            return Task.FromResult((token, expiresAtUtc));
        }
    }
}
