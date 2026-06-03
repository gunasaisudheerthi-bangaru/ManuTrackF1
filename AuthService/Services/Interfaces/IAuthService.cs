using AuthService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace AuthService.Services.Interfaces;

/// <summary>
/// All methods return ApiResponse — no exceptions thrown for business errors.
/// </summary>
public interface IAuthService
{
    Task<ApiResponse<LoginResponse>>       LoginAsync(LoginRequest request);
    Task<ApiResponse<AuthUserViewModel>>   RegisterAsync(RegisterRequest request);
    Task<ApiResponse<AuthUserViewModel>>   GetByIdAsync(int id);
    Task<ApiResponse<List<AuthUserViewModel>>> GetAllAsync();
    Task<ApiResponse>                      ChangePasswordAsync(int userId, ChangePasswordRequest request);
    Task<bool>                             EmailExistsAsync(string email);
    Task<ApiResponse>                      DeactivateUserAsync(int userId);
    Task<ApiResponse>                      ActivateUserAsync(int userId);
    Task<ApiResponse<AuthUserViewModel>>   UpdateUserAsync(int userId, UpdateUserRequest request);
}
