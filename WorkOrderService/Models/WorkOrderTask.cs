namespace WorkOrderService.Models;

public class WorkOrderTask
{
    public int TaskID { get; set; }
    public int WorkOrderID { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? CompletedDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public WorkOrder? WorkOrder { get; set; }
}
