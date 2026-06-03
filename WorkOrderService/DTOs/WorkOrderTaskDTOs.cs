using System.ComponentModel.DataAnnotations;

namespace WorkOrderService.DTOs;

public class CreateWorkOrderTaskRequest
{
    [Required(ErrorMessage = "WorkOrderID is required.")]
    [Range(1, int.MaxValue)]
    public int WorkOrderID { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [MinLength(5)][MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "AssignedTo is required.")]
    [MinLength(2)][MaxLength(200)]
    public string AssignedTo { get; set; } = string.Empty;
}

// Kept for backward compatibility
public class UpdateWorkOrderTaskRequest { }

public class UpdateTaskStatusRequest
{
    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Pending|InProgress|Completed|Cancelled)$",
        ErrorMessage = "Status must be one of: Pending, InProgress, Completed, Cancelled.")]
    public string Status { get; set; } = string.Empty;
}

public class WorkOrderTaskViewModel
{
    public int TaskID { get; set; }
    public int WorkOrderID { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
