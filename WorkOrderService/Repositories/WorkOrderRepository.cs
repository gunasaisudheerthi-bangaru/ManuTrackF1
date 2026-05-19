using Microsoft.EntityFrameworkCore;
using WorkOrderService.Data;
using WorkOrderService.Models;
using WorkOrderService.Repositories.Interfaces;

namespace WorkOrderService.Repositories;

public class WorkOrderRepository(WorkOrderDbContext db) : IWorkOrderRepository
{
    public async Task<IEnumerable<WorkOrder>> GetAllAsync(string? status = null, int? productId = null)
    {
        var query = db.WorkOrders.Include(w => w.Tasks).AsQueryable();
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(w => w.Status == status);
        if (productId.HasValue)
            query = query.Where(w => w.ProductID == productId.Value);
        return await query.OrderByDescending(w => w.CreatedDate).ToListAsync();
    }

    public async Task<WorkOrder?> GetByIdAsync(int id) =>
        await db.WorkOrders.FindAsync(id);

    public async Task<WorkOrder?> GetByIdWithTasksAsync(int id) =>
        await db.WorkOrders.Include(w => w.Tasks)
                           .FirstOrDefaultAsync(w => w.WorkOrderID == id);

    public async Task<WorkOrder> CreateAsync(WorkOrder workOrder)
    {
        db.WorkOrders.Add(workOrder);
        await db.SaveChangesAsync();
        return workOrder;
    }

    public async Task<WorkOrder> UpdateAsync(WorkOrder workOrder)
    {
        db.WorkOrders.Update(workOrder);
        await db.SaveChangesAsync();
        return workOrder;
    }

    public async Task DeleteAsync(WorkOrder workOrder)
    {
        db.WorkOrders.Remove(workOrder);
        await db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await db.WorkOrders.AnyAsync(w => w.WorkOrderID == id);
}
