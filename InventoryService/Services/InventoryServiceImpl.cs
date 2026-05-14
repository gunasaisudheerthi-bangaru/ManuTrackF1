using InventoryService.DTOs;
using InventoryService.Models;
using InventoryService.Repositories.Interfaces;
using InventoryService.Services.Interfaces;
using InventoryService.Enums;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;

namespace InventoryService.Services;

public class InventoryServiceImpl(IInventoryRepository repo) : IInventoryService
{
    public async Task<ApiResponse<IEnumerable<InventoryItemViewModel>>> GetAllAsync(string? status, string? locationId)
    {
        var items = await repo.GetAllAsync(status, locationId);
        return ApiResponse<IEnumerable<InventoryItemViewModel>>.Ok(items.Select(Map));
    }

    public async Task<ApiResponse<InventoryItemViewModel>> GetByIdAsync(int id)
    {
        var item = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Inventory item {id} not found.");
        return ApiResponse<InventoryItemViewModel>.Ok(Map(item));
    }

    public async Task<ApiResponse<IEnumerable<InventoryItemViewModel>>> GetLowStockAsync()
    {
        var items = await repo.GetLowStockAsync();
        return ApiResponse<IEnumerable<InventoryItemViewModel>>.Ok(items.Select(Map));
    }

    public async Task<ApiResponse<InventoryItemViewModel>> CreateAsync(CreateInventoryItemRequest request)
    {
        var item = new InventoryItem
        {
            ProductID = request.ProductID,
            ProductName = request.ProductName,
            LocationID = request.LocationID,
            QuantityOnHand = request.QuantityOnHand,
            MinimumQuantity = request.MinimumQuantity,
            Notes = request.Notes,
            Status = DetermineStatus(request.QuantityOnHand, request.MinimumQuantity),
            CreatedDate = DateTime.UtcNow
        };

        var created = await repo.CreateAsync(item);
        return ApiResponse<InventoryItemViewModel>.Ok(Map(created), "Inventory item created.");
    }

    public async Task<ApiResponse<InventoryItemViewModel>> UpdateAsync(int id, UpdateInventoryItemRequest request)
    {
        var item = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Inventory item {id} not found.");

        if (request.LocationID != null) item.LocationID = request.LocationID;
        if (request.QuantityOnHand.HasValue) item.QuantityOnHand = request.QuantityOnHand.Value;
        if (request.MinimumQuantity.HasValue) item.MinimumQuantity = request.MinimumQuantity.Value;
        if (request.Notes != null) item.Notes = request.Notes;
        item.Status = DetermineStatus(item.QuantityOnHand, item.MinimumQuantity);
        item.ModifiedDate = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(item);
        return ApiResponse<InventoryItemViewModel>.Ok(Map(updated), "Inventory item updated.");
    }

    public async Task<ApiResponse<InventoryItemViewModel>> AdjustQuantityAsync(int id, AdjustQuantityRequest request)
    {
        var item = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Inventory item {id} not found.");

        item.QuantityOnHand += request.Adjustment;
        if (item.QuantityOnHand < 0)
            throw new ValidationException("Adjustment would result in negative stock.");

        item.Status = DetermineStatus(item.QuantityOnHand, item.MinimumQuantity);
        item.ModifiedDate = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(item);
        return ApiResponse<InventoryItemViewModel>.Ok(Map(updated), "Quantity adjusted.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var item = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Inventory item {id} not found.");
        await repo.DeleteAsync(item);
        return ApiResponse.Ok("Inventory item deleted.");
    }

    private static string DetermineStatus(decimal qty, decimal minQty)
    {
        if (qty <= 0) return InventoryStatus.OutOfStock;
        if (qty <= minQty) return InventoryStatus.LowStock;
        return InventoryStatus.Available;
    }

    private static InventoryItemViewModel Map(InventoryItem i) => new()
    {
        InventoryID = i.InventoryID,
        ProductID = i.ProductID,
        ProductName = i.ProductName,
        LocationID = i.LocationID,
        QuantityOnHand = i.QuantityOnHand,
        MinimumQuantity = i.MinimumQuantity,
        Status = i.Status,
        Notes = i.Notes,
        CreatedDate = i.CreatedDate,
        ModifiedDate = i.ModifiedDate
    };
}
