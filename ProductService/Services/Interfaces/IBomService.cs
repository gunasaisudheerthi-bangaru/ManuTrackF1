using ManuTrack.SharedKernel.Responses;
using ProductService.DTOs;

namespace ProductService.Services.Interfaces;

public interface IBomService
{
    Task<ApiResponse<IEnumerable<BomViewModel>>> GetAllBomsAsync(int? productId, string? status);
    Task<ApiResponse<BomViewModel>> GetBomByIdAsync(int id);
    Task<ApiResponse<IEnumerable<BomViewModel>>> GetBomsByProductIdAsync(int productId);
    Task<ApiResponse<BomViewModel>> CreateBomAsync(CreateBomRequest request);
    Task<ApiResponse<BomViewModel>> UpdateBomAsync(int id, UpdateBomRequest request);
    Task<ApiResponse<BomViewModel>> UpdateBomStatusAsync(int id, UpdateBomStatusRequest request);
    Task<ApiResponse> DeleteBomAsync(int id);
}
