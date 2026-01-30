using ChalanaChithram.AuthService.Api.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChalanaChithram.AuthService.Api.Helpers;

public class JwtTokenHelper
{
    private readonly IConfiguration configuration;

    public JwtTokenHelper(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateAccessToken(AppUser user)
    {
        string? issuer = configuration["JwtSettings:Issuer"];
        string? audience = configuration["JwtSettings:Audience"];
        string? key = configuration["JwtSettings:Key"];
        string? minutesValue = configuration["JwtSettings:AccessTokenMinutes"];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException("JWT Key is missing.");
        }

        int minutes = 15;
        if (!string.IsNullOrWhiteSpace(minutesValue))
        {
            minutes = Convert.ToInt32(minutesValue);
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("fullName", user.FullName)
        };

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        byte[] bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}
