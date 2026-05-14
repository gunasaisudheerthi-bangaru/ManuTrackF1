using InventoryService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace InventoryService.Services.Interfaces;

public interface IPurchaseOrderService
{
    Task<ApiResponse<IEnumerable<PurchaseOrderViewModel>>> GetAllAsync(string? status);
    Task<ApiResponse<PurchaseOrderViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<PurchaseOrderViewModel>> CreateAsync(CreatePurchaseOrderRequest request);
    Task<ApiResponse<PurchaseOrderViewModel>> UpdateStatusAsync(int id, UpdatePurchaseOrderStatusRequest request);
}
