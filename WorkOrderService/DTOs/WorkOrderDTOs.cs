using System.ComponentModel.DataAnnotations;

namespace WorkOrderService.DTOs;

public class CreateWorkOrderRequest : IValidatableObject
{
    [Required(ErrorMessage = "ProductID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductID must be a positive integer.")]
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, 1000000, ErrorMessage = "Quantity must be between 1 and 1,000,000.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate <= StartDate)
            yield return new ValidationResult("End date must be after start date.", [nameof(EndDate)]);
    }
}

// Kept for backward compatibility — no fields needed since we only update status
public class UpdateWorkOrderRequest { }

public class UpdateWorkOrderStatusRequest
{
    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Pending|Scheduled|InProgress|Completed|Cancelled)$",
        ErrorMessage = "Status must be one of: Pending, Scheduled, InProgress, Completed, Cancelled.")]
    public string Status { get; set; } = string.Empty;
}

public class WorkOrderViewModel
{
    public int WorkOrderID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsOverdue => Status != "Completed" && Status != "Cancelled" && EndDate < DateTime.UtcNow;
    public int TaskCount { get; set; }
}
