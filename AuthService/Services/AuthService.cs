using AuthService.DTOs;
using AuthService.Models;
using AuthService.Repositories.Interfaces;
using AuthService.Services.Interfaces;
using AuthService.Enums;
using ManuTrack.SharedKernel.Helpers;
using ManuTrack.SharedKernel.Responses;

namespace AuthService.Services;

public class AuthServiceImpl : IAuthService
{
    private readonly IAuthRepository _repo;
    private readonly IConfiguration  _config;
    private readonly AuditClient      _audit;

    public AuthServiceImpl(IAuthRepository repo, IConfiguration config, AuditClient audit)
    {
        _repo   = repo;
        _config = config;
        _audit  = audit;
    }

    // ── LOGIN ─────────────────────────────────────────────────────────────────
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var email = request.Email?.Trim().ToLower() ?? string.Empty;
        var user  = await _repo.GetByEmailAsync(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return ApiResponse<LoginResponse>.Fail("Invalid email or password.");

        if (!user.IsActive)
            return ApiResponse<LoginResponse>.Fail("Your account has been deactivated. Please contact admin.");

        var token = JwtHelper.GenerateToken(
            userId:        user.UserID,
            email:         user.Email,
            role:          user.Role,
            name:          user.Name,
            secretKey:     _config["Jwt:Key"]!,
            issuer:        _config["Jwt:Issuer"]!,
            audience:      _config["Jwt:Audience"]!,
            expiryMinutes: 60);

        _ = _audit.LogAsync(user.UserID, user.Name, "Login", "User", user.UserID.ToString(),
            $"User '{user.Email}' logged in successfully.");

        return ApiResponse<LoginResponse>.Ok(
            new LoginResponse(token, user.Role, user.Name, user.UserID, user.Email),
            "Login successful.");
    }

    // ── REGISTER ──────────────────────────────────────────────────────────────
    public async Task<ApiResponse<AuthUserViewModel>> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email?.Trim().ToLower() ?? string.Empty;

        if (await _repo.EmailExistsAsync(email))
            return ApiResponse<AuthUserViewModel>.Fail($"Email '{email}' is already registered.");

        var validRoles = new[] {
            AppRoles.Admin, AppRoles.Planner, AppRoles.Operator,
            AppRoles.Inspector, AppRoles.InventoryManager, AppRoles.ComplianceOfficer
        };
        if (!validRoles.Contains(request.Role))
            return ApiResponse<AuthUserViewModel>.Fail($"Invalid role '{request.Role}'.");

        var user = new AuthUser
        {
            Name         = request.Name.Trim(),
            Email        = email,
            Phone        = request.Phone.Trim(),
            Role         = request.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 4),
            IsActive     = true
        };

        var created = await _repo.CreateAsync(user);

        _ = _audit.LogAsync(created.UserID, "System Admin", "Register", "User", created.UserID.ToString(),
            $"New user '{created.Email}' registered with role '{created.Role}'.");

        return ApiResponse<AuthUserViewModel>.Ok(MapToViewModel(created), "User registered successfully.");
    }

    // ── GET BY ID ─────────────────────────────────────────────────────────────
    public async Task<ApiResponse<AuthUserViewModel>> GetByIdAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null)
            return ApiResponse<AuthUserViewModel>.Fail($"User {id} not found.");
        return ApiResponse<AuthUserViewModel>.Ok(MapToViewModel(user));
    }

    // ── GET ALL ───────────────────────────────────────────────────────────────
    public async Task<ApiResponse<List<AuthUserViewModel>>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return ApiResponse<List<AuthUserViewModel>>.Ok(
            users.Select(MapToViewModel).ToList());
    }

    // ── CHANGE PASSWORD ───────────────────────────────────────────────────────
    public async Task<ApiResponse> ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null)
            return ApiResponse.Fail("User not found.");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return ApiResponse.Fail("Current password is incorrect.");

        if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
            return ApiResponse.Fail("New password cannot be the same as current password.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, workFactor: 4);
        await _repo.UpdateAsync(user);
        return ApiResponse.Ok("Password changed successfully.");
    }

    // ── DEACTIVATE ────────────────────────────────────────────────────────────
    public async Task<ApiResponse> DeactivateUserAsync(int userId)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null) return ApiResponse.Fail("User not found.");
        if (!user.IsActive) return ApiResponse.Fail("User is already deactivated.");

        user.IsActive = false;
        await _repo.UpdateAsync(user);

        _ = _audit.LogAsync(userId, "System Admin", "Deactivate", "User", userId.ToString(),
            $"User '{user.Email}' was deactivated.");

        return ApiResponse.Ok("User deactivated successfully.");
    }

    // ── ACTIVATE ──────────────────────────────────────────────────────────────
    public async Task<ApiResponse> ActivateUserAsync(int userId)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null) return ApiResponse.Fail("User not found.");
        if (user.IsActive) return ApiResponse.Fail("User is already active.");

        user.IsActive = true;
        await _repo.UpdateAsync(user);

        _ = _audit.LogAsync(userId, "System Admin", "Activate", "User", userId.ToString(),
            $"User '{user.Email}' was activated.");

        return ApiResponse.Ok("User activated successfully.");
    }

    // ── UPDATE USER ───────────────────────────────────────────────────────────
    public async Task<ApiResponse<AuthUserViewModel>> UpdateUserAsync(int userId, UpdateUserRequest request)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null) return ApiResponse<AuthUserViewModel>.Fail("User not found.");

        if (!string.IsNullOrWhiteSpace(request.Name))  user.Name  = request.Name.Trim();
        if (!string.IsNullOrWhiteSpace(request.Phone)) user.Phone = request.Phone.Trim();
        if (!string.IsNullOrWhiteSpace(request.Role))  user.Role  = request.Role;

        await _repo.UpdateAsync(user);

        _ = _audit.LogAsync(userId, "System Admin", "UpdateUser", "User", userId.ToString(),
            $"Updated user '{user.Email}'.");

        return ApiResponse<AuthUserViewModel>.Ok(MapToViewModel(user), "User updated successfully.");
    }

    // ── EMAIL CHECK ───────────────────────────────────────────────────────────
    public async Task<bool> EmailExistsAsync(string email) =>
        await _repo.EmailExistsAsync(email.Trim().ToLower());

    // ── MAPPER ────────────────────────────────────────────────────────────────
    private static AuthUserViewModel MapToViewModel(AuthUser user) =>
        new(user.UserID, user.Name, user.Role, user.Email, user.Phone, user.IsActive);
}
