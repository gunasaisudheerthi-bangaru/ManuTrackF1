namespace QualityService.Models;

public class Defect
{
    public int DefectID { get; set; }
    public int InspectionID { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Resolution { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public Inspection? Inspection { get; set; }
}
