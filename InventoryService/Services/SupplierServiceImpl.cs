using InventoryService.DTOs;
using InventoryService.Models;
using InventoryService.Repositories.Interfaces;
using InventoryService.Services.Interfaces;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;

namespace InventoryService.Services;

public class SupplierServiceImpl(ISupplierRepository repo) : ISupplierService
{
    public async Task<ApiResponse<IEnumerable<SupplierViewModel>>> GetAllAsync(bool? isActive)
    {
        var suppliers = await repo.GetAllAsync(isActive);
        return ApiResponse<IEnumerable<SupplierViewModel>>.Ok(suppliers.Select(Map));
    }

    public async Task<ApiResponse<SupplierViewModel>> GetByIdAsync(int id)
    {
        var supplier = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Supplier {id} not found.");
        return ApiResponse<SupplierViewModel>.Ok(Map(supplier));
    }

    public async Task<ApiResponse<SupplierViewModel>> CreateAsync(CreateSupplierRequest request)
    {
        var supplier = new Supplier
        {
            Name = request.Name,
            ContactPerson = request.ContactPerson,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        var created = await repo.CreateAsync(supplier);
        return ApiResponse<SupplierViewModel>.Ok(Map(created), "Supplier created successfully.");
    }

    public async Task<ApiResponse<SupplierViewModel>> UpdateAsync(int id, UpdateSupplierRequest request)
    {
        var supplier = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Supplier {id} not found.");

        if (request.Name != null) supplier.Name = request.Name;
        if (request.ContactPerson != null) supplier.ContactPerson = request.ContactPerson;
        if (request.Phone != null) supplier.Phone = request.Phone;
        if (request.Email != null) supplier.Email = request.Email;
        if (request.Address != null) supplier.Address = request.Address;
        if (request.IsActive.HasValue) supplier.IsActive = request.IsActive.Value;
        supplier.ModifiedDate = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(supplier);
        return ApiResponse<SupplierViewModel>.Ok(Map(updated), "Supplier updated successfully.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var supplier = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Supplier {id} not found.");

        if (await repo.HasPurchaseOrdersAsync(id))
            throw new ConflictException($"Cannot delete supplier '{supplier.Name}' because it has linked purchase orders.");

        await repo.DeleteAsync(supplier);
        return ApiResponse.Ok("Supplier deleted successfully.");
    }

    private static SupplierViewModel Map(Supplier s) => new()
    {
        SupplierID = s.SupplierID,
        Name = s.Name,
        ContactPerson = s.ContactPerson,
        Phone = s.Phone,
        Email = s.Email,
        Address = s.Address,
        IsActive = s.IsActive,
        CreatedDate = s.CreatedDate,
        ModifiedDate = s.ModifiedDate
    };
}
