using ChalanaChithram.AuthService.Api.DTOs.Requests;
using ChalanaChithram.AuthService.Api.DTOs.Responses;
using System.Threading.Tasks;

namespace ChalanaChithram.AuthService.Api.Services.Interfaces;

public interface IAuthService
{
    Task Register(RegisterRequest request);
    Task<AuthResponse> Login(LoginRequest request);
}
