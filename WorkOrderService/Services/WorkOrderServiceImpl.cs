using WorkOrderService.Enums;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;
using WorkOrderService.DTOs;
using WorkOrderService.Models;
using WorkOrderService.Repositories.Interfaces;
using WorkOrderService.Services.Interfaces;

namespace WorkOrderService.Services;

public class WorkOrderServiceImpl(IWorkOrderRepository repo) : IWorkOrderService
{
    public async Task<ApiResponse<IEnumerable<WorkOrderViewModel>>> GetAllAsync(string? status, int? productId)
    {
        var orders = await repo.GetAllAsync(status, productId);
        return ApiResponse<IEnumerable<WorkOrderViewModel>>.Ok(orders.Select(Map));
    }

    public async Task<ApiResponse<WorkOrderViewModel>> GetByIdAsync(int id)
    {
        var order = await repo.GetByIdWithTasksAsync(id)
            ?? throw new NotFoundException($"WorkOrder {id} not found.");
        return ApiResponse<WorkOrderViewModel>.Ok(Map(order));
    }

    public async Task<ApiResponse<WorkOrderViewModel>> CreateAsync(CreateWorkOrderRequest request)
    {
        if (request.EndDate <= request.StartDate)
            throw new ValidationException("EndDate must be after StartDate.");

        var workOrder = new WorkOrder
        {
            ProductID = request.ProductID,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AssignedTo = request.AssignedTo,
            AssignedOperatorID = request.AssignedOperatorID,
            CreatedBy = request.CreatedBy,
            Notes = request.Notes,
            Status = WorkOrderStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        var created = await repo.CreateAsync(workOrder);
        return ApiResponse<WorkOrderViewModel>.Ok(Map(created), "Work order created successfully.");
    }

    public async Task<ApiResponse<WorkOrderViewModel>> UpdateAsync(int id, UpdateWorkOrderRequest request)
    {
        var order = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"WorkOrder {id} not found.");

        if (request.Quantity.HasValue) order.Quantity = request.Quantity.Value;
        if (request.StartDate.HasValue) order.StartDate = request.StartDate.Value;
        if (request.EndDate.HasValue) order.EndDate = request.EndDate.Value;
        if (request.AssignedTo != null) order.AssignedTo = request.AssignedTo;
        if (request.AssignedOperatorID.HasValue) order.AssignedOperatorID = request.AssignedOperatorID.Value;
        if (request.CreatedBy != null) order.CreatedBy = request.CreatedBy;
        if (request.Notes != null) order.Notes = request.Notes;
        order.ModifiedDate = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(order);
        return ApiResponse<WorkOrderViewModel>.Ok(Map(updated), "Work order updated successfully.");
    }

    public async Task<ApiResponse<WorkOrderViewModel>> UpdateStatusAsync(int id, UpdateWorkOrderStatusRequest request)
    {
        var order = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"WorkOrder {id} not found.");

        order.Status = request.Status;
        order.ModifiedDate = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(order);
        return ApiResponse<WorkOrderViewModel>.Ok(Map(updated), "Work order status updated.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var order = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"WorkOrder {id} not found.");

        await repo.DeleteAsync(order);
        return ApiResponse.Ok("Work order deleted successfully.");
    }

    private static WorkOrderViewModel Map(WorkOrder w) => new()
    {
        WorkOrderID = w.WorkOrderID,
        ProductID = w.ProductID,
        ProductName = w.ProductName,
        Quantity = w.Quantity,
        StartDate = w.StartDate,
        EndDate = w.EndDate,
        Status = w.Status,
        AssignedTo = w.AssignedTo,
        AssignedOperatorID = w.AssignedOperatorID,
        CreatedBy = w.CreatedBy,
        Notes = w.Notes,
        CreatedDate = w.CreatedDate,
        ModifiedDate = w.ModifiedDate,
        TaskCount = w.Tasks.Count
    };
}
