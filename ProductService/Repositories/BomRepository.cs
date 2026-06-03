using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Repositories.Interfaces;

namespace ProductService.Repositories;

public class BomRepository(ProductDbContext db) : IBomRepository
{
    public async Task<IEnumerable<Bom>> GetAllAsync(int? productId = null, string? status = null)
    {
        var query = db.Boms.Include(b => b.Product).Include(b => b.Component).AsQueryable();
        if (productId.HasValue)
            query = query.Where(b => b.ProductID == productId.Value);
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(b => b.Status == status);
        return await query.OrderBy(b => b.BOMID).ToListAsync();
    }

    public async Task<Bom?> GetByIdAsync(int id) =>
        await db.Boms.Include(b => b.Product).Include(b => b.Component)
                     .FirstOrDefaultAsync(b => b.BOMID == id);

    public async Task<IEnumerable<Bom>> GetByProductIdAsync(int productId) =>
        await db.Boms.Include(b => b.Component)
                     .Where(b => b.ProductID == productId)
                     .ToListAsync();

    public async Task<Bom> CreateAsync(Bom bom)
    {
        db.Boms.Add(bom);
        await db.SaveChangesAsync();
        return bom;
    }

    public async Task<Bom> UpdateAsync(Bom bom)
    {
        db.Boms.Update(bom);
        await db.SaveChangesAsync();
        return bom;
    }

    public async Task DeleteAsync(Bom bom)
    {
        db.Boms.Remove(bom);
        await db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await db.Boms.AnyAsync(b => b.BOMID == id);

    public async Task<int> CountForProductAsync(int productId) =>
        await db.Boms.CountAsync(b => b.ProductID == productId);

    public async Task DeleteAllForProductAsync(int productId)
    {
        var boms = await db.Boms.Where(b => b.ProductID == productId).ToListAsync();
        db.Boms.RemoveRange(boms);
        await db.SaveChangesAsync();
    }

    public async Task<bool> HasBomsForComponentAsync(int componentId) =>
        await db.Boms.AnyAsync(b => b.ComponentID == componentId);
}
