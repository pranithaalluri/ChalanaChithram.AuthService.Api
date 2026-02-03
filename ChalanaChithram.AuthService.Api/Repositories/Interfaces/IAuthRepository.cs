using ChalanaChithram.AuthService.Api.Entities;
using System.Threading.Tasks;

namespace ChalanaChithram.AuthService.Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<bool> UserExistsByEmail(string email);
    Task<AppUser?> GetUserByEmail(string email);
    Task AddUser(AppUser user);
    Task AddRefreshToken(RefreshToken refreshToken);
    Task SaveChanges();
}
