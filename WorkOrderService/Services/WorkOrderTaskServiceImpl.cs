using WorkOrderService.Enums;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;
using WorkOrderService.DTOs;
using WorkOrderService.Models;
using WorkOrderService.Repositories.Interfaces;
using WorkOrderService.Services.Interfaces;

namespace WorkOrderService.Services;

public class WorkOrderTaskServiceImpl(IWorkOrderTaskRepository taskRepo, IWorkOrderRepository workOrderRepo) : IWorkOrderTaskService
{
    public async Task<ApiResponse<IEnumerable<WorkOrderTaskViewModel>>> GetByWorkOrderIdAsync(int workOrderId)
    {
        if (!await workOrderRepo.ExistsAsync(workOrderId))
            throw new NotFoundException($"WorkOrder {workOrderId} not found.");

        var tasks = await taskRepo.GetByWorkOrderIdAsync(workOrderId);
        return ApiResponse<IEnumerable<WorkOrderTaskViewModel>>.Ok(tasks.Select(Map));
    }

    public async Task<ApiResponse<WorkOrderTaskViewModel>> GetByIdAsync(int id)
    {
        var task = await taskRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Task {id} not found.");
        return ApiResponse<WorkOrderTaskViewModel>.Ok(Map(task));
    }

    public async Task<ApiResponse<WorkOrderTaskViewModel>> CreateAsync(CreateWorkOrderTaskRequest request)
    {
        if (!await workOrderRepo.ExistsAsync(request.WorkOrderID))
            throw new NotFoundException($"WorkOrder {request.WorkOrderID} not found.");

        var task = new WorkOrderTask
        {
            WorkOrderID = request.WorkOrderID,
            Description = request.Description,
            AssignedTo = request.AssignedTo,
            Notes = request.Notes,
            Status = WorkOrderTaskStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        var created = await taskRepo.CreateAsync(task);
        return ApiResponse<WorkOrderTaskViewModel>.Ok(Map(created), "Task created successfully.");
    }

    public async Task<ApiResponse<WorkOrderTaskViewModel>> UpdateAsync(int id, UpdateWorkOrderTaskRequest request)
    {
        var task = await taskRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Task {id} not found.");

        if (request.Description != null) task.Description = request.Description;
        if (request.AssignedTo != null) task.AssignedTo = request.AssignedTo;
        if (request.Notes != null) task.Notes = request.Notes;
        task.UpdatedDate = DateTime.UtcNow;

        var updated = await taskRepo.UpdateAsync(task);
        return ApiResponse<WorkOrderTaskViewModel>.Ok(Map(updated), "Task updated successfully.");
    }

    public async Task<ApiResponse<WorkOrderTaskViewModel>> UpdateStatusAsync(int id, UpdateTaskStatusRequest request)
    {
        var task = await taskRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Task {id} not found.");

        task.Status = request.Status;
        if (request.Status == WorkOrderTaskStatus.Completed)
            task.CompletedDate = DateTime.UtcNow;
        task.UpdatedDate = DateTime.UtcNow;

        var updated = await taskRepo.UpdateAsync(task);
        return ApiResponse<WorkOrderTaskViewModel>.Ok(Map(updated), "Task status updated.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var task = await taskRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Task {id} not found.");

        await taskRepo.DeleteAsync(task);
        return ApiResponse.Ok("Task deleted successfully.");
    }

    private static WorkOrderTaskViewModel Map(WorkOrderTask t) => new()
    {
        TaskID = t.TaskID,
        WorkOrderID = t.WorkOrderID,
        Description = t.Description,
        AssignedTo = t.AssignedTo,
        Status = t.Status,
        CompletedDate = t.CompletedDate,
        Notes = t.Notes,
        CreatedDate = t.CreatedDate,
        UpdatedDate = t.UpdatedDate
    };
}
