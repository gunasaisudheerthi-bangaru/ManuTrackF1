using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using ProductService.Enums;
using ManuTrack.SharedKernel.Helpers;
using ManuTrack.SharedKernel.Responses;
using ProductService.DTOs;
using ProductService.Models;
using ProductService.Repositories.Interfaces;
using ProductService.Services.Interfaces;

namespace ProductService.Services;

public class ProductServiceImpl(
    IProductRepository repo,
    IBomRepository bomRepo,
    IHttpClientFactory httpClientFactory,
    IHttpContextAccessor httpContextAccessor,
    ILogger<ProductServiceImpl> logger) : IProductService
{
    // ── Change 4: Audit logging (fire-and-forget) ────────────────────────────
    private async Task LogAuditAsync(string action, string entityType, string entityId, string? details = null)
    {
        try
        {
            var (userId, userName) = ServiceHelper.GetCurrentUser(httpContextAccessor);
            if (userId == 0) return;

            var client = ServiceHelper.CreateAuthorizedClient(httpClientFactory, httpContextAccessor, "ComplianceService");
            await client.PostAsJsonAsync("api/v1/audit", new
            {
                UserID = userId,
                UserName = userName,
                Action = action,
                EntityType = entityType,
                EntityID = entityId,
                ServiceName = "ProductService",
                Details = details
            });
        }
        catch (Exception ex) { logger.LogWarning(ex, "Audit log failed in ProductService."); }
    }

    // ── CRUD ────────────────────────────────────────────────────────────────

    public async Task<ApiResponse<IEnumerable<ProductViewModel>>> GetAllProductsAsync(string? category, string? status)
    {
        var products = await repo.GetAllAsync(category, status);
        return ApiResponse<IEnumerable<ProductViewModel>>.Ok(products.Select(Map));
    }

    public async Task<ApiResponse<ProductViewModel>> GetProductByIdAsync(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null)
            return ApiResponse<ProductViewModel>.Fail($"Product {id} not found.");
        return ApiResponse<ProductViewModel>.Ok(Map(product));
    }

    public async Task<ApiResponse<ProductViewModel>> CreateProductAsync(CreateProductRequest request)
    {
        var existing = await repo.GetByNameAsync(request.Name);
        if (existing != null)
            return ApiResponse<ProductViewModel>.Fail($"Product with name '{request.Name}' already exists.");

        var product = new Product
        {
            Name = request.Name,
            Category = request.Category,
            Version = request.Version,
            Description = request.Description,
            Status = ProductStatus.Draft,
            
        };

        var created = await repo.CreateAsync(product);

        // Change 4: audit log
        await LogAuditAsync("Created Product", "Product", created.ProductID.ToString(),
            $"Name: {created.Name}, Category: {created.Category}");

        return ApiResponse<ProductViewModel>.Ok(Map(created), "Product created successfully.");
    }

    public async Task<ApiResponse<ProductViewModel>> UpdateProductAsync(int id, UpdateProductRequest request)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null)
            return ApiResponse<ProductViewModel>.Fail($"Product {id} not found.");

        if (request.Name != null) product.Name = request.Name;
        if (request.Category != null) product.Category = request.Category;
        if (request.Version != null) product.Version = request.Version;
        if (request.Description != null) product.Description = request.Description;

        var updated = await repo.UpdateAsync(product);

        // Change 4: audit log
        await LogAuditAsync("Updated Product", "Product", id.ToString(),
            $"Name: {updated.Name}, Status: {updated.Status}");

        return ApiResponse<ProductViewModel>.Ok(Map(updated), "Product updated successfully.");
    }

    public async Task<ApiResponse<ProductViewModel>> UpdateProductStatusAsync(int id, UpdateProductStatusRequest request)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null)
            return ApiResponse<ProductViewModel>.Fail($"Product {id} not found.");

        product.Status = request.Status;

        var updated = await repo.UpdateAsync(product);

        // Change 4: audit log
        await LogAuditAsync("Updated Product Status", "Product", id.ToString(),
            $"New Status: {request.Status}");

        // Cancel all active work orders when product is discontinued
        if (request.Status == ProductStatus.Discontinued)
            _ = CancelActiveWorkOrdersAsync(id, product.Name);

        return ApiResponse<ProductViewModel>.Ok(Map(updated), "Product status updated.");
    }

    private async Task CancelActiveWorkOrdersAsync(int productId, string productName)
    {
        try
        {
            var woClient = ServiceHelper.CreateAuthorizedClient(httpClientFactory, httpContextAccessor, "WorkOrderService");

            var response = await woClient.GetAsync($"api/v1/workorders?productId={productId}");
            if (!response.IsSuccessStatusCode) return;

            var result = await response.Content
                .ReadFromJsonAsync<WorkOrderListResponseDto>(
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var activeOrders = result?.Data?
                .Where(w => w.Status == "Pending" || w.Status == "InProgress")
                .ToList();

            if (activeOrders == null || activeOrders.Count == 0) return;

            foreach (var wo in activeOrders)
                await woClient.PutAsJsonAsync($"api/v1/workorders/{wo.WorkOrderID}/status", new { Status = "Cancelled" });

            var notifyClient = ServiceHelper.CreateAuthorizedClient(httpClientFactory, httpContextAccessor, "NotificationService");
            await notifyClient.PostAsJsonAsync("api/v1/notifications/notify-role", new
            {
                TargetRole = "Planner",
                Title = $"Work Orders Cancelled — {productName} Discontinued",
                Message = $"{activeOrders.Count} active work order(s) for \"{productName}\" have been automatically cancelled because the product was discontinued.",
                Category = "WorkOrder",
                Priority = "High"
            });

            logger.LogInformation("Cancelled {Count} work orders for discontinued product {ProductId}.", activeOrders.Count, productId);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to cancel work orders for discontinued product {ProductId}.", productId);
        }
    }

    private sealed class WorkOrderListResponseDto { public IEnumerable<WorkOrderSummaryDto>? Data { get; set; } }
    private sealed class WorkOrderSummaryDto { public int WorkOrderID { get; set; } public string Status { get; set; } = string.Empty; }

    public async Task<ApiResponse> DeleteProductAsync(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null)
            return ApiResponse.Fail($"Product {id} not found.");

        // Delete associated BOM entries first to avoid FK constraint violation
        await bomRepo.DeleteAllForProductAsync(id);

        await repo.DeleteAsync(product);

        // Change 4: audit log
        await LogAuditAsync("Deleted Product", "Product", id.ToString(),
            $"Name: {product.Name}");

        return ApiResponse.Ok("Product deleted successfully.");
    }

    // ── Mapper ──────────────────────────────────────────────────────────────

    private static ProductViewModel Map(Product p) => new()
    {
        ProductID = p.ProductID,
        Name = p.Name,
        Category = p.Category,
        Version = p.Version,
        Status = p.Status,
        Description = p.Description,
        
    };
}
