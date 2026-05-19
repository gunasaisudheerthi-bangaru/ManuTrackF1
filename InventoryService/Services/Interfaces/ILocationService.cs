using InventoryService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace InventoryService.Services.Interfaces;

public interface ILocationService
{
    Task<ApiResponse<IEnumerable<LocationViewModel>>> GetAllAsync(bool? isActive = null);
    Task<ApiResponse<LocationViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<LocationViewModel>> CreateAsync(CreateLocationRequest request);
    Task<ApiResponse<LocationViewModel>> UpdateAsync(int id, UpdateLocationRequest request);
    Task<ApiResponse> DeleteAsync(int id);
}
