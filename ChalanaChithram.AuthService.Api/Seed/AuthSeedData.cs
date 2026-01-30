using System;
using ChalanaChithram.AuthService.Api.Entities;
using ChalanaChithram.AuthService.Api.Helpers;

namespace ChalanaChithram.AuthService.Api.Seed;

public static class AuthSeedData
{

    public static AppUser GetDefaultUser()
    {
        return new AppUser
        {
            FullName = "POC Admin",
            Email = "admin@chalanachithram.com",
            PasswordHash = PasswordHasher.HashPassword("Admin@123"),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}

