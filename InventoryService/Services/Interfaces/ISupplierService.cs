using InventoryService.DTOs;
using ManuTrack.SharedKernel.Responses;

namespace InventoryService.Services.Interfaces;

public interface ISupplierService
{
    Task<ApiResponse<IEnumerable<SupplierViewModel>>> GetAllAsync(bool? isActive);
    Task<ApiResponse<SupplierViewModel>> GetByIdAsync(int id);
    Task<ApiResponse<SupplierViewModel>> CreateAsync(CreateSupplierRequest request);
    Task<ApiResponse<SupplierViewModel>> UpdateAsync(int id, UpdateSupplierRequest request);
    Task<ApiResponse> DeleteAsync(int id);
}
