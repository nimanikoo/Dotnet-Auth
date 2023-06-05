using Core.Application.Contracts;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUser(LoginUser user)
    {
        var result = await _authService.RegisterUser(user);
        if (result)
        {
            return Ok("Successfully Registered");
        }

        return BadRequest("Something went wrong!");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUser user)
    {
        if (!ModelState.IsValid) return BadRequest();
        var result = await _authService.Login(user);
        var token = _authService.GenerateToken(user);
        return result ? Ok(token) : BadRequest();
    }
}