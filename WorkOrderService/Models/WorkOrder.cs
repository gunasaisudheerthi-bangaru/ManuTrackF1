namespace WorkOrderService.Models;

public class WorkOrder
{
    public int WorkOrderID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public ICollection<WorkOrderTask> Tasks { get; set; } = new List<WorkOrderTask>();
}
