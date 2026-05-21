using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Application.DTO.Request.Auth;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared.Response;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _authService.LogoutAsync(userId);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }
}