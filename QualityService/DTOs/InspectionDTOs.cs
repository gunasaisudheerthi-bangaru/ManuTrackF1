using System.ComponentModel.DataAnnotations;

namespace QualityService.DTOs;

public class CreateInspectionRequest : IValidatableObject
{
    [Required(ErrorMessage = "WorkOrderID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "WorkOrderID must be a positive integer.")]
    public int WorkOrderID { get; set; }

    [Required(ErrorMessage = "Inspection date is required.")]
    public DateTime InspectionDate { get; set; }

    [Required(ErrorMessage = "Inspector ID is required.")]
    [MinLength(2, ErrorMessage = "Inspector ID must be at least 2 characters.")]
    [MaxLength(100, ErrorMessage = "Inspector ID cannot exceed 100 characters.")]
    public string InspectorID { get; set; } = string.Empty;

    [Required(ErrorMessage = "Inspector name is required.")]
    [MinLength(2, ErrorMessage = "Inspector name must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Inspector name cannot exceed 200 characters.")]
    public string InspectorName { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (InspectionDate.Date < DateTime.UtcNow.Date)
            yield return new ValidationResult(
                "Inspection date cannot be in the past.",
                [nameof(InspectionDate)]);
    }
}

public class UpdateInspectionResultRequest
{
    [Required(ErrorMessage = "Result is required.")]
    [RegularExpression("^(Pass|Fail)$", ErrorMessage = "Result must be Pass or Fail.")]
    public string Result { get; set; } = string.Empty;

    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Scheduled|InProgress|Completed|Cancelled)$",
        ErrorMessage = "Status must be one of: Scheduled, InProgress, Completed, Cancelled.")]
    public string Status { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
    public string? Notes { get; set; }
}

public class InspectionViewModel
{
    public int InspectionID { get; set; }
    public int WorkOrderID { get; set; }
    public DateTime InspectionDate { get; set; }
    public string InspectorID { get; set; } = string.Empty;
    public string InspectorName { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int TotalDefectCount { get; set; }
    public int CriticalCount { get; set; }
    public int HighCount { get; set; }
    public int MediumCount { get; set; }
    public int LowCount { get; set; }
}
