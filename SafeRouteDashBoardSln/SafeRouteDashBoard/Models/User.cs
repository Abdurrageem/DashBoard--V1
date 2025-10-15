namespace SafeRouteDashBoard.Models
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public List<string> Permissions { get; set; } = new();
        public UserPreferences Preferences { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    public enum UserRole
    {
        Admin,
        Manager,
        Dispatcher,
        Viewer
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended,
        PendingActivation
    }

    public class UserPreferences
    {
        public bool DarkMode { get; set; }
        public string Language { get; set; } = "en-US";
        public string TimeZone { get; set; } = "UTC";
        public string DateFormat { get; set; } = "MM/dd/yyyy";
        public string TimeFormat { get; set; } = "12h";
        public bool ShowTutorial { get; set; } = true;
        public NotificationPreferences NotificationPreferences { get; set; } = new();
        public DashboardPreferences DashboardPreferences { get; set; } = new();
    }

    public class DashboardPreferences
    {
        public bool ShowMap { get; set; } = true;
        public bool ShowDriverSidebar { get; set; } = true;
        public bool ShowKpiCards { get; set; } = true;
        public int RefreshInterval { get; set; } = 5; // seconds
        public List<string> EnabledWidgets { get; set; } = new();
    }
}
