namespace NotificationService.Models;

public class Notification
{
    public int NotificationID { get; set; }
    public int UserID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    // Change 5: priority column
    public string Priority { get; set; } = "Medium";
    // Change 6: optional expiry
    public DateTime? ExpiryDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? ReadDate { get; set; }
}
