namespace AuthService.Services;

public class AuditClient(IHttpClientFactory httpClientFactory, ILogger<AuditClient> logger)
{
    private readonly HttpClient _http = httpClientFactory.CreateClient("ComplianceService");

    public async Task LogAsync(int userId, string userName, string action, string entityType, string entityId, string? details = null)
    {
        try
        {
            var payload = new
            {
                UserID      = userId,
                UserName    = userName,
                Action      = action,
                EntityType  = entityType,
                EntityID    = entityId,
                ServiceName = "AuthService",
                Details     = details
            };

            var response = await _http.PostAsJsonAsync("/api/v1/audit", payload);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                logger.LogWarning("Audit log failed [{Status}]: {Body}", response.StatusCode, body);
            }
        }
        catch (Exception ex)
        {
            // Audit failure must not break the primary operation
            logger.LogWarning(ex, "Audit logging error for action '{Action}'", action);
        }
    }
}
