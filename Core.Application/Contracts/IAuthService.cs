using Core.Domain.Entities;

namespace Core.Application.Contracts;

public interface IAuthService
{
    Task<bool> RegisterUser(LoginUser user);
    Task<bool> Login(LoginUser user);
    string GenerateToken(LoginUser user);
}