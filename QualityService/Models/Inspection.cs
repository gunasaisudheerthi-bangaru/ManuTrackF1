namespace QualityService.Models;

public class Inspection
{
    public int InspectionID { get; set; }
    public int WorkOrderID { get; set; }
    public DateTime InspectionDate { get; set; }
    public string InspectorID { get; set; } = string.Empty;
    public string InspectorName { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public ICollection<Defect> Defects { get; set; } = new List<Defect>();
}
