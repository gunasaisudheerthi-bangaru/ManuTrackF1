using InventoryService.Data;
using InventoryService.Models;
using InventoryService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Repositories;

public class StockMovementRepository(InventoryDbContext db) : IStockMovementRepository
{
    public async Task<IEnumerable<StockMovement>> GetByInventoryIdAsync(int inventoryId) =>
        await db.StockMovements
            .Where(m => m.InventoryID == inventoryId)
            .OrderByDescending(m => m.CreatedDate)
            .ToListAsync();

    public async Task<StockMovement> CreateAsync(StockMovement movement)
    {
        db.StockMovements.Add(movement);
        await db.SaveChangesAsync();
        return movement;
    }

    public async Task<bool> HasMovementsAsync(int inventoryId) =>
        await db.StockMovements.AnyAsync(m => m.InventoryID == inventoryId);
}
