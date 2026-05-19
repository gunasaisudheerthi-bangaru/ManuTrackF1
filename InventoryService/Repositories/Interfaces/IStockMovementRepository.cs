using InventoryService.Models;

namespace InventoryService.Repositories.Interfaces;

public interface IStockMovementRepository
{
    Task<IEnumerable<StockMovement>> GetByInventoryIdAsync(int inventoryId);
    Task<StockMovement> CreateAsync(StockMovement movement);
    Task<bool> HasMovementsAsync(int inventoryId);
}
