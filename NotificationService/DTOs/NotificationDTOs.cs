using System.ComponentModel.DataAnnotations;

namespace NotificationService.DTOs;

public class SendNotificationRequest
{
    [Required(ErrorMessage = "UserID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "UserID must be a positive number.")]
    public int UserID { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required.")]
    [MinLength(5, ErrorMessage = "Message must be at least 5 characters.")]
    [MaxLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
    public string Message { get; set; } = string.Empty;

    // Change 1: category validation
    [Required(ErrorMessage = "Category is required.")]
    [RegularExpression("^(WorkOrder|Inventory|Quality|Compliance|General)$",
        ErrorMessage = "Category must be one of: WorkOrder, Inventory, Quality, Compliance, General.")]
    public string Category { get; set; } = string.Empty;

    // Change 5: optional priority, defaults to Medium
    [RegularExpression("^(Low|Medium|High|Critical)$",
        ErrorMessage = "Priority must be one of: Low, Medium, High, Critical.")]
    public string Priority { get; set; } = "Medium";
}

public class BroadcastNotificationRequest
{
    [Required(ErrorMessage = "At least one UserID is required.")]
    [MinLength(1, ErrorMessage = "UserIDs list must contain at least one entry.")]
    [MaxLength(500, ErrorMessage = "UserIDs list cannot exceed 500 entries.")]
    public List<int> UserIDs { get; set; } = [];

    [Required(ErrorMessage = "Title is required.")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters.")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required.")]
    [MinLength(5, ErrorMessage = "Message must be at least 5 characters.")]
    [MaxLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
    public string Message { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    [RegularExpression("^(WorkOrder|Inventory|Quality|Compliance|General)$",
        ErrorMessage = "Category must be one of: WorkOrder, Inventory, Quality, Compliance, General.")]
    public string Category { get; set; } = string.Empty;

    // Change 5: optional priority
    [RegularExpression("^(Low|Medium|High|Critical)$",
        ErrorMessage = "Priority must be one of: Low, Medium, High, Critical.")]
    public string Priority { get; set; } = "Medium";
}

// Change 2 + 5 + 6: full details in response
public class NotificationViewModel
{
    public int NotificationID { get; set; }
    public int UserID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? ReadDate { get; set; }
}

// Change 4: unread count broken down by category
public class UnreadCountViewModel
{
    public int Total { get; set; }
    public Dictionary<string, int> ByCategory { get; set; } = [];
}
