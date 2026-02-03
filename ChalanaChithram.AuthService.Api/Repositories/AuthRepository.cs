using ChalanaChithram.AuthService.Api.Data;
using ChalanaChithram.AuthService.Api.Entities;
using ChalanaChithram.AuthService.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChalanaChithram.AuthService.Api.Repositories;

public class AuthRepository(AppDbContext dbContext) : IAuthRepository
{
    private readonly AppDbContext dbContext = dbContext;

    public async Task<bool> UserExistsByEmail(string email)
    {
        return await dbContext.AppUsers.AnyAsync(x => x.Email == email);
    }

    public async Task<AppUser?> GetUserByEmail(string email)
    {
        return await dbContext.AppUsers.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddUser(AppUser user)
    {
        dbContext.AppUsers.Add(user);
        await Task.CompletedTask;
    }

    public async Task AddRefreshToken(RefreshToken refreshToken)
    {
        dbContext.RefreshTokens.Add(refreshToken);
        await Task.CompletedTask;
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}
