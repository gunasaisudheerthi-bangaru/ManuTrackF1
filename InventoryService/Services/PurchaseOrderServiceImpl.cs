using InventoryService.DTOs;
using InventoryService.Models;
using InventoryService.Repositories.Interfaces;
using InventoryService.Services.Interfaces;
using InventoryService.Enums;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;

namespace InventoryService.Services;

public class PurchaseOrderServiceImpl(IPurchaseOrderRepository repo) : IPurchaseOrderService
{
    public async Task<ApiResponse<IEnumerable<PurchaseOrderViewModel>>> GetAllAsync(string? status)
    {
        var orders = await repo.GetAllAsync(status);
        return ApiResponse<IEnumerable<PurchaseOrderViewModel>>.Ok(orders.Select(Map));
    }

    public async Task<ApiResponse<PurchaseOrderViewModel>> GetByIdAsync(int id)
    {
        var po = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Purchase order {id} not found.");
        return ApiResponse<PurchaseOrderViewModel>.Ok(Map(po));
    }

    public async Task<ApiResponse<PurchaseOrderViewModel>> CreateAsync(CreatePurchaseOrderRequest request)
    {
        var items = request.Items.Select(i => new PurchaseOrderItem
        {
            ProductID = i.ProductID,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList();

        var totalAmount = items.Sum(i => i.Quantity * i.UnitPrice);

        var po = new PurchaseOrder
        {
            SupplierID = request.SupplierID,
            SupplierName = request.SupplierName,
            OrderDate = DateTime.UtcNow,
            ExpectedDeliveryDate = request.ExpectedDeliveryDate,
            Notes = request.Notes,
            TotalAmount = totalAmount,
            Status = PurchaseOrderStatus.Pending,
            CreatedDate = DateTime.UtcNow,
            Items = items
        };

        var created = await repo.CreateAsync(po);
        return ApiResponse<PurchaseOrderViewModel>.Ok(Map(created), "Purchase order created.");
    }

    public async Task<ApiResponse<PurchaseOrderViewModel>> UpdateStatusAsync(int id, UpdatePurchaseOrderStatusRequest request)
    {
        var po = await repo.GetByIdAsync(id) ?? throw new NotFoundException($"Purchase order {id} not found.");
        po.Status = request.Status;
        po.ModifiedDate = DateTime.UtcNow;
        var updated = await repo.UpdateAsync(po);
        return ApiResponse<PurchaseOrderViewModel>.Ok(Map(updated), "Purchase order status updated.");
    }

    private static PurchaseOrderViewModel Map(PurchaseOrder p) => new()
    {
        POID = p.POID,
        SupplierID = p.SupplierID,
        SupplierName = p.SupplierName,
        OrderDate = p.OrderDate,
        ExpectedDeliveryDate = p.ExpectedDeliveryDate,
        Status = p.Status,
        TotalAmount = p.TotalAmount,
        Notes = p.Notes,
        CreatedDate = p.CreatedDate,
        Items = p.Items.Select(i => new PurchaseOrderItemViewModel
        {
            ItemID = i.ItemID,
            ProductID = i.ProductID,
            ProductName = i.ProductName,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            CreatedDate = i.CreatedDate
        }).ToList()
    };
}
