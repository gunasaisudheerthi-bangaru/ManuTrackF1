using System.ComponentModel.DataAnnotations;

namespace QualityService.DTOs;

public class CreateDefectRequest
{
    [Required(ErrorMessage = "InspectionID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "InspectionID must be a positive integer.")]
    public int InspectionID { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [MinLength(5, ErrorMessage = "Description must be at least 5 characters.")]
    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Severity is required.")]
    [RegularExpression("^(Low|Medium|High|Critical)$",
        ErrorMessage = "Severity must be one of: Low, Medium, High, Critical.")]
    public string Severity { get; set; } = string.Empty;
}

public class ResolveDefectRequest
{
    [Required(ErrorMessage = "Resolution description is required.")]
    [MinLength(10, ErrorMessage = "Resolution description must be at least 10 characters.")]
    [MaxLength(1000, ErrorMessage = "Resolution description cannot exceed 1000 characters.")]
    public string ResolutionDescription { get; set; } = string.Empty;
}

public class UpdateDefectStatusRequest
{
    [Required(ErrorMessage = "Status is required.")]
    [RegularExpression("^(Open|InReview|Resolved|Closed)$",
        ErrorMessage = "Status must be one of: Open, InReview, Resolved, Closed.")]
    public string Status { get; set; } = string.Empty;
}

public class DefectViewModel
{
    public int DefectID { get; set; }
    public int InspectionID { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ResolutionDescription { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
}
