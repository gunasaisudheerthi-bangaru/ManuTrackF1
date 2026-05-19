using InventoryService.Models;

namespace InventoryService.Repositories.Interfaces;

public interface ISupplierRepository
{
    Task<IEnumerable<Supplier>> GetAllAsync(bool? isActive = null);
    Task<Supplier?> GetByIdAsync(int id);
    Task<Supplier> CreateAsync(Supplier supplier);
    Task<Supplier> UpdateAsync(Supplier supplier);
    Task DeleteAsync(Supplier supplier);
    Task<bool> ExistsAsync(int id);
    Task<bool> HasPurchaseOrdersAsync(int id);
}
