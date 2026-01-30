using ChalanaChithram.AuthService.Api.Data;
using ChalanaChithram.AuthService.Api.DTOs.Requests;
using ChalanaChithram.AuthService.Api.DTOs.Responses;
using ChalanaChithram.AuthService.Api.Entities;
using ChalanaChithram.AuthService.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChalanaChithram.AuthService.Api.Controllers;

[ApiController]

[Route("api/[controller]")]
public class AuthController(AppDbContext dbContext, JwtTokenHelper jwtTokenHelper) : ControllerBase
{
    private readonly AppDbContext dbContext = dbContext;
    private readonly JwtTokenHelper jwtTokenHelper = jwtTokenHelper;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email and Password are required.");
        }

        bool exists = await dbContext.AppUsers.AnyAsync(x => x.Email == request.Email);
        if (exists)
        {
            return Conflict("User already exists with this email.");
        }

        AppUser user = new AppUser
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = PasswordHasher.HashPassword(request.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.AppUsers.Add(user);
        await dbContext.SaveChangesAsync();

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        AppUser? user = await dbContext.AppUsers.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        bool passwordOk = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!passwordOk)
        {
            return Unauthorized("Invalid email or password.");
        }

        string accessToken = jwtTokenHelper.GenerateAccessToken(user);
        string refreshTokenValue = jwtTokenHelper.GenerateRefreshToken();

        RefreshToken refreshToken = new()
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            RevokedAt = null
        };

        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync();

        AuthResponse response = new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };

        return Ok(response);
    }
}
