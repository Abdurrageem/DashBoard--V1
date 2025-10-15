namespace SafeRouteDashBoard.Models
{
    /// <summary>
    /// Represents a system notification for users
    /// </summary>
    public class Notification
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string RelatedEntityId { get; set; } = string.Empty;
        public string RelatedEntityType { get; set; } = string.Empty; // "Driver", "Route", "Alert", etc.
        public string ActionUrl { get; set; } = string.Empty;
        
        public string GetRelativeTime()
        {
            var timeSpan = DateTime.Now - Timestamp;
            
            if (timeSpan.TotalMinutes < 1)
                return "Just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}m ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours}h ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays}d ago";
            
            return Timestamp.ToString("MMM dd, yyyy");
        }
    }

    public enum NotificationType
    {
        Critical,
        Warning,
        Info,
        Success
    }

    public class NotificationPreferences
    {
        public bool EmailNotifications { get; set; }
        public bool SmsAlerts { get; set; }
        public bool PushNotifications { get; set; }
        public bool DesktopNotifications { get; set; }
        public bool SoundEnabled { get; set; }
        
        // Alert thresholds
        public int CriticalAlertThreshold { get; set; }
        public int HighRiskScoreThreshold { get; set; }
        public int LateDeliveryMinutes { get; set; }
    }
}
