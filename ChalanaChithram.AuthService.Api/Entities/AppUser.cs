using System;
using System.Collections.Generic;

namespace ChalanaChithram.AuthService.Api.Entities;

public class AppUser
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
