using System.ComponentModel.DataAnnotations;

namespace InventoryService.DTOs;

public class CreateInventoryItemRequest
{
    [Required(ErrorMessage = "ProductID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductID must be a positive integer.")]
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [MinLength(2, ErrorMessage = "Product name must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    public string ProductName { get; set; } = string.Empty;

    // Changed: nullable int FK to InventoryLocation
    [Range(1, int.MaxValue, ErrorMessage = "LocationID must be a positive integer.")]
    public int? LocationID { get; set; }

    [Required(ErrorMessage = "Quantity on hand is required.")]
    [Range(0, 9999999.9999, ErrorMessage = "Quantity on hand must be between 0 and 9,999,999.")]
    public decimal QuantityOnHand { get; set; }

    [Range(0, 9999999.9999, ErrorMessage = "Minimum quantity must be between 0 and 9,999,999.")]
    public decimal MinimumQuantity { get; set; }

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}

public class UpdateInventoryItemRequest
{
    // Changed: nullable int FK to InventoryLocation
    [Range(1, int.MaxValue, ErrorMessage = "LocationID must be a positive integer.")]
    public int? LocationID { get; set; }

    [Range(0, 9999999.9999, ErrorMessage = "Quantity on hand must be between 0 and 9,999,999.")]
    public decimal? QuantityOnHand { get; set; }

    [Range(0, 9999999.9999, ErrorMessage = "Minimum quantity must be between 0 and 9,999,999.")]
    public decimal? MinimumQuantity { get; set; }

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}

public class AdjustQuantityRequest
{
    [Required(ErrorMessage = "Adjustment value is required.")]
    [Range(-9999999.9999, 9999999.9999, ErrorMessage = "Adjustment must be between -9,999,999 and 9,999,999.")]
    public decimal Adjustment { get; set; }

    [Required(ErrorMessage = "Reason for adjustment is required.")]
    [MinLength(5, ErrorMessage = "Reason must be at least 5 characters.")]
    [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters.")]
    public string Reason { get; set; } = string.Empty;
}

public class InventoryItemViewModel
{
    public int InventoryID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    // Changed: int? + LocationName from navigation property
    public int? LocationID { get; set; }
    public string? LocationName { get; set; }
    public decimal QuantityOnHand { get; set; }
    public decimal MinimumQuantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
