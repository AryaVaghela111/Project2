using MiniPM.Api.DTOs.Auth;
using MiniPM.Api.Models;

namespace MiniPM.Api.Services
{
    public interface IAuthService
    {
        Task<User?> Register(RegisterDto dto);
        Task<string?> Login(LoginDto dto);
    }
}
