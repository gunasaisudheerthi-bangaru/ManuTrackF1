using AuthService.DTOs;
using AuthService.Services.Interfaces;
using ManuTrack.SharedKernel.Helpers;
using ManuTrack.SharedKernel.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result.Success) return Conflict(result);
        return Created($"api/v1/auth/users/{result.Data!.UserID}", result);
    }

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
        => Ok(await _authService.GetAllAsync());

    [HttpGet("users/{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetUser(int id)
    {
        if (id <= 0) return BadRequest(ApiResponse.Fail("User ID must be a positive number."));
        var result = await _authService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPut("users/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        if (id <= 0) return BadRequest(ApiResponse.Fail("User ID must be a positive number."));
        var result = await _authService.UpdateUserAsync(id, request);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = JwtHelper.GetUserId(User);
        var result = await _authService.ChangePasswordAsync(userId, request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("users/{id:int}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateUser(int id)
    {
        if (id <= 0) return BadRequest(ApiResponse.Fail("User ID must be a positive number."));
        if (id == JwtHelper.GetUserId(User))
            return BadRequest(ApiResponse.Fail("You cannot deactivate your own account."));
        var result = await _authService.DeactivateUserAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("users/{id:int}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActivateUser(int id)
    {
        if (id <= 0) return BadRequest(ApiResponse.Fail("User ID must be a positive number."));
        var result = await _authService.ActivateUserAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
