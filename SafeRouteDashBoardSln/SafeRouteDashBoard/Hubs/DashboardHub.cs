using Microsoft.AspNetCore.SignalR;
using SafeRouteDashBoard.Models;

namespace SafeRouteDashBoard.Hubs
{
    /// <summary>
    /// SignalR Hub for real-time dashboard updates
    /// </summary>
    public class DashboardHub : Hub
    {
        private readonly ILogger<DashboardHub> _logger;

        public DashboardHub(ILogger<DashboardHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Update driver location in real-time
        /// </summary>
        public async Task UpdateDriverLocation(string driverId, double latitude, double longitude)
        {
            await Clients.All.SendAsync("DriverLocationUpdated", driverId, latitude, longitude);
        }

        /// <summary>
        /// Update delivery count
        /// </summary>
        public async Task UpdateDeliveryCount(int count, double onTimePercentage)
        {
            await Clients.All.SendAsync("DeliveryCountUpdated", count, onTimePercentage);
        }

        /// <summary>
        /// Send new alert to all connected clients
        /// </summary>
        public async Task SendAlert(Alert alert)
        {
            await Clients.All.SendAsync("NewAlert", alert);
        }

        /// <summary>
        /// Broadcast system status update
        /// </summary>
        public async Task BroadcastSystemStatus(string status, DateTime lastUpdate)
        {
            await Clients.All.SendAsync("SystemStatusUpdated", status, lastUpdate);
        }

        /// <summary>
        /// Notify user action (for audit log)
        /// </summary>
        public async Task NotifyUserAction(string userId, string action, string entityType, string entityId)
        {
            await Clients.All.SendAsync("UserActionNotified", userId, action, entityType, entityId);
        }

        /// <summary>
        /// Update driver status
        /// </summary>
        public async Task UpdateDriverStatus(string driverId, string status)
        {
            await Clients.All.SendAsync("DriverStatusUpdated", driverId, status);
        }

        /// <summary>
        /// Broadcast metric update
        /// </summary>
        public async Task BroadcastMetricUpdate(string metricName, object value)
        {
            await Clients.All.SendAsync("MetricUpdated", metricName, value);
        }
    }
}
