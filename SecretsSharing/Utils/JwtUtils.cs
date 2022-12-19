using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using SecretsSharing.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace SecretsSharing.Utils
{
    public class JwtUtils
    {
       
        private readonly IConfiguration _configuration;

        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var token = CreateJwtToken(CreateClaims(user),CreateSigningCredentials());

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials)
        {
            return new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"],
                                        audience: _configuration["Jwt:Audience"],
                                        claims,
                                        expires: DateTime.Now.AddHours(3),
                                        signingCredentials: credentials);
        }

        private Claim[] CreateClaims(User user)
        {
            return new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                            new Claim(ClaimTypes.Email, user.Email)
                         };
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(new SymmetricSecurityKey
                                         (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                                         SecurityAlgorithms.HmacSha256);
        }
    }
}
