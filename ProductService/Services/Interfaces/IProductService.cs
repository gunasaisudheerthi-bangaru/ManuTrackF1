using ManuTrack.SharedKernel.Responses;
using ProductService.DTOs;

namespace ProductService.Services.Interfaces;

public interface IProductService
{
    Task<ApiResponse<IEnumerable<ProductViewModel>>> GetAllProductsAsync(string? category, string? status);
    Task<ApiResponse<ProductViewModel>> GetProductByIdAsync(int id);
    Task<ApiResponse<ProductViewModel>> CreateProductAsync(CreateProductRequest request);
    Task<ApiResponse<ProductViewModel>> UpdateProductAsync(int id, UpdateProductRequest request);
    Task<ApiResponse<ProductViewModel>> UpdateProductStatusAsync(int id, UpdateProductStatusRequest request);
    Task<ApiResponse> DeleteProductAsync(int id);
}
