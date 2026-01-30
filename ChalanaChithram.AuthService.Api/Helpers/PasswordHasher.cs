using System;
using System.Security.Cryptography;
using System.Text;

namespace ChalanaChithram.AuthService.Api.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string passwordHash)
    {
        string computedHash = HashPassword(password);
        return string.Equals(computedHash, passwordHash, StringComparison.Ordinal);
    }
}
