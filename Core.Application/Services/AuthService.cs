using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Application.Contracts;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> RegisterUser(LoginUser user)
    {
        var identityUser = new IdentityUser
        {
            UserName = user.UserName,
            Email = user.UserName
        };

        IdentityResult userResult = await _userManager.CreateAsync(identityUser, user.Password);
        return userResult.Succeeded;
    }

    public async Task<bool> Login(LoginUser user)
    {
        var identityUser = await _userManager.FindByEmailAsync(user.UserName);
        return identityUser is not null && await _userManager.CheckPasswordAsync(identityUser, user.Password);
    }

    public string GenerateToken(LoginUser user)
    {
        IEnumerable<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.UserName),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTConfig:Key").Value ??
                                                            throw new InvalidOperationException()));
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha512Signature
        );

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            audience: _configuration.GetSection("JWTConfig:Issuer").Value,
            issuer: _configuration.GetSection("JWTConfig:Audience").Value,
            signingCredentials: signingCredentials
        );
        if (securityToken == null) throw new ArgumentNullException(nameof(securityToken));
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken) ??
                    throw new ArgumentNullException(nameof(user));
        if (token == null) throw new ArgumentNullException(nameof(token));
        return token;
    }
}