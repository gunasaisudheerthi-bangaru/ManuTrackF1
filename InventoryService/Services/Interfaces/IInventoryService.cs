using InventoryService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace InventoryService.Services.Interfaces;

public interface IInventoryService
{
    Task<ApiResponse<IEnumerable<InventoryItemViewModel>>> GetAllAsync(string? status, string? locationId);
    Task<ApiResponse<InventoryItemViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<IEnumerable<InventoryItemViewModel>>> GetLowStockAsync();
    Task<ApiResponse<InventoryItemViewModel>> CreateAsync(CreateInventoryItemRequest request);
    Task<ApiResponse<InventoryItemViewModel>> UpdateAsync(int id, UpdateInventoryItemRequest request);
    Task<ApiResponse<InventoryItemViewModel>> AdjustQuantityAsync(int id, AdjustQuantityRequest request);
    Task<ApiResponse> DeleteAsync(int id);
}
