using SafeRouteDashBoard.Models;

namespace SafeRouteDashBoard.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsAsync(string userId);
        Task MarkAsReadAsync(string notificationId);
        Task SendNotificationAsync(Notification notification);
        Task ClearAllNotificationsAsync(string userId);
        Task<NotificationPreferences> GetNotificationPreferencesAsync(string userId);
        Task UpdatePreferencesAsync(string userId, NotificationPreferences preferences);
        int GetUnreadCount(string userId);
    }

    public class NotificationService : INotificationService
    {
        private readonly List<Notification> _notifications;
        private readonly Dictionary<string, NotificationPreferences> _preferences;
        private readonly Random _random = new();

        public NotificationService()
        {
            _notifications = GenerateMockNotifications();
            _preferences = new Dictionary<string, NotificationPreferences>();
        }

        public async Task<List<Notification>> GetNotificationsAsync(string userId)
        {
            await Task.Delay(100);
            return _notifications
                .Where(n => n.UserId == userId || string.IsNullOrEmpty(n.UserId))
                .OrderByDescending(n => n.Timestamp)
                .Take(10)
                .ToList();
        }

        public async Task MarkAsReadAsync(string notificationId)
        {
            await Task.Delay(50);
            var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
            }
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            await Task.Delay(50);
            notification.Id = $"notif-{_notifications.Count + 1:D3}";
            notification.Timestamp = DateTime.Now;
            notification.IsRead = false;
            _notifications.Add(notification);
        }

        public async Task ClearAllNotificationsAsync(string userId)
        {
            await Task.Delay(50);
            _notifications.RemoveAll(n => n.UserId == userId);
        }

        public async Task<NotificationPreferences> GetNotificationPreferencesAsync(string userId)
        {
            await Task.Delay(50);
            if (!_preferences.ContainsKey(userId))
            {
                _preferences[userId] = new NotificationPreferences
                {
                    EmailNotifications = true,
                    SmsAlerts = true,
                    PushNotifications = true,
                    DesktopNotifications = true,
                    SoundEnabled = true,
                    CriticalAlertThreshold = 3,
                    HighRiskScoreThreshold = 85,
                    LateDeliveryMinutes = 30
                };
            }
            return _preferences[userId];
        }

        public async Task UpdatePreferencesAsync(string userId, NotificationPreferences preferences)
        {
            await Task.Delay(50);
            _preferences[userId] = preferences;
        }

        public int GetUnreadCount(string userId)
        {
            return _notifications.Count(n => !n.IsRead && (n.UserId == userId || string.IsNullOrEmpty(n.UserId)));
        }

        private List<Notification> GenerateMockNotifications()
        {
            return new List<Notification>
            {
                new()
                {
                    Id = "notif-001",
                    Title = "Critical Alert",
                    Message = "Emergency button pressed by Emily Watson in Sandton area",
                    Type = NotificationType.Critical,
                    Timestamp = DateTime.Now.AddMinutes(-7),
                    IsRead = false,
                    UserId = "user-001",
                    RelatedEntityId = "driver-004",
                    RelatedEntityType = "Driver"
                },
                new()
                {
                    Id = "notif-002",
                    Title = "Maintenance Required",
                    Message = "Vehicle VEH-103 requires immediate maintenance at Midrand depot",
                    Type = NotificationType.Warning,
                    Timestamp = DateTime.Now.AddMinutes(-17),
                    IsRead = false,
                    UserId = "user-001",
                    RelatedEntityId = "VEH-103",
                    RelatedEntityType = "Vehicle"
                },
                new()
                {
                    Id = "notif-003",
                    Title = "Delivery Complete",
                    Message = "Route SR-2024-001 completed successfully - all Johannesburg deliveries done",
                    Type = NotificationType.Success,
                    Timestamp = DateTime.Now.AddHours(-1),
                    IsRead = false,
                    UserId = "user-001",
                    RelatedEntityId = "SR-2024-001",
                    RelatedEntityType = "Route"
                }
            };
        }
    }
}
