using Microsoft.EntityFrameworkCore;
using QualityService.Data;
using QualityService.Models;
using QualityService.Repositories.Interfaces;

namespace QualityService.Repositories;

public class DefectRepository(QualityDbContext db) : IDefectRepository
{
    public async Task<IEnumerable<Defect>> GetByInspectionIdAsync(int inspectionId) =>
        await db.Defects.Where(d => d.InspectionID == inspectionId).ToListAsync();

    public async Task<IEnumerable<Defect>> GetAllAsync(string? status = null, string? severity = null)
    {
        var query = db.Defects.AsQueryable();
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(d => d.Status == status);
        if (!string.IsNullOrWhiteSpace(severity)) query = query.Where(d => d.Severity == severity);
        return await query.OrderByDescending(d => d.CreatedDate).ToListAsync();
    }

    public async Task<Defect?> GetByIdAsync(int id) => await db.Defects.FindAsync(id);

    public async Task<Defect> CreateAsync(Defect defect)
    {
        db.Defects.Add(defect);
        await db.SaveChangesAsync();
        return defect;
    }

    public async Task<Defect> UpdateAsync(Defect defect)
    {
        db.Defects.Update(defect);
        await db.SaveChangesAsync();
        return defect;
    }

    public async Task<bool> ExistsAsync(int id) => await db.Defects.AnyAsync(d => d.DefectID == id);
}
