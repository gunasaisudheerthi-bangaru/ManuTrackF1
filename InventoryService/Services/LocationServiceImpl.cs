using InventoryService.DTOs;
using InventoryService.Models;
using InventoryService.Repositories.Interfaces;
using InventoryService.Services.Interfaces;
using ManuTrack.SharedKernel.Exceptions;
using ManuTrack.SharedKernel.Responses;

namespace InventoryService.Services;

public class LocationServiceImpl(ILocationRepository repo) : ILocationService
{
    public async Task<ApiResponse<IEnumerable<LocationViewModel>>> GetAllAsync(bool? isActive = null)
    {
        var locations = await repo.GetAllAsync(isActive);
        return ApiResponse<IEnumerable<LocationViewModel>>.Ok(locations.Select(Map));
    }

    public async Task<ApiResponse<LocationViewModel>> GetByIdAsync(int id)
    {
        var location = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Location {id} not found.");
        return ApiResponse<LocationViewModel>.Ok(Map(location));
    }

    public async Task<ApiResponse<LocationViewModel>> CreateAsync(CreateLocationRequest request)
    {
        var location = new InventoryLocation
        {
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        var created = await repo.CreateAsync(location);
        return ApiResponse<LocationViewModel>.Ok(Map(created), "Location created.");
    }

    public async Task<ApiResponse<LocationViewModel>> UpdateAsync(int id, UpdateLocationRequest request)
    {
        var location = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Location {id} not found.");

        if (request.Name != null)        location.Name = request.Name;
        if (request.Description != null) location.Description = request.Description;
        if (request.IsActive.HasValue)   location.IsActive = request.IsActive.Value;
        location.ModifiedDate = DateTime.UtcNow;

        var updated = await repo.UpdateAsync(location);
        return ApiResponse<LocationViewModel>.Ok(Map(updated), "Location updated.");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var location = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Location {id} not found.");

        if (await repo.HasItemsAsync(id))
            throw new ValidationException(
                $"Cannot delete location '{location.Name}' because it still has inventory items assigned. " +
                "Reassign or remove those items first.");

        await repo.DeleteAsync(location);
        return ApiResponse.Ok($"Location '{location.Name}' deleted.");
    }

    private static LocationViewModel Map(InventoryLocation l) => new()
    {
        LocationID   = l.LocationID,
        Name         = l.Name,
        Description  = l.Description,
        IsActive     = l.IsActive,
        CreatedDate  = l.CreatedDate,
        ModifiedDate = l.ModifiedDate
    };
}
