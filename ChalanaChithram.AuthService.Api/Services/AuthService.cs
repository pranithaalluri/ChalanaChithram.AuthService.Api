using ChalanaChithram.AuthService.Api.DTOs.Requests;
using ChalanaChithram.AuthService.Api.DTOs.Responses;
using ChalanaChithram.AuthService.Api.Entities;
using ChalanaChithram.AuthService.Api.Helpers;
using ChalanaChithram.AuthService.Api.Repositories;
using ChalanaChithram.AuthService.Api.Repositories.Interfaces;
using ChalanaChithram.AuthService.Api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ChalanaChithram.AuthService.Api.Services;

public class AuthService(
    IAuthRepository authRepository,
    JwtTokenHelper jwtTokenHelper
) : IAuthService
{
    private readonly IAuthRepository authRepository = authRepository;
    private readonly JwtTokenHelper jwtTokenHelper = jwtTokenHelper;

    public async Task Register(RegisterRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Email and Password are required.");
            }

            bool exists = await authRepository.UserExistsByEmail(request.Email);
            if (exists)
            {
                throw new InvalidOperationException("User already exists with this email.");
            }

            AppUser user = new AppUser
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await authRepository.AddUser(user);
            await authRepository.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public async Task<AuthResponse> Login(LoginRequest request)
    {
        try
        {
            AppUser? user = await authRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email ");
            }

            bool passwordOk =
                PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);

            if (!passwordOk)
            {
                throw new UnauthorizedAccessException("Invalid password");
            }

            string accessToken = jwtTokenHelper.GenerateAccessToken(user);
            string refreshTokenValue = jwtTokenHelper.GenerateRefreshToken();

            RefreshToken refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await authRepository.AddRefreshToken(refreshToken);
            await authRepository.SaveChanges();

            return new AuthResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            };
        }
        catch
        {
            throw;
        }
    }
}
