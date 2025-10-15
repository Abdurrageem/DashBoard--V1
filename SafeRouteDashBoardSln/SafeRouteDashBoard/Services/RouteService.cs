using SafeRouteDashBoard.Models;

namespace SafeRouteDashBoard.Services
{
    public interface IRouteService
    {
        Task<List<Models.Route>> GetActiveRoutesAsync();
        Task<Models.Route?> GetRouteByIdAsync(string id);
        Task<List<Models.Route>> GetRoutesByDriverAsync(string driverId);
        Task<Models.Route> CreateRouteAsync(Models.Route route);
        Task<Models.Route> UpdateRouteAsync(Models.Route route);
        Task<Models.Route> OptimizeRouteAsync(string routeId);
        Task<int> GetRouteProgressAsync(string routeId);
        Task UpdateStopStatusAsync(string routeId, int stopIndex, StopStatus status);
    }

    public class RouteService : IRouteService
    {
        private readonly List<Models.Route> _routes;
        private readonly Random _random = new();

        public RouteService()
        {
            _routes = GenerateMockRoutes();
        }

        public async Task<List<Models.Route>> GetActiveRoutesAsync()
        {
            await Task.Delay(100);
            return _routes.Where(r => r.Status == RouteStatus.InProgress || r.Status == RouteStatus.Pending).ToList();
        }

        public async Task<Models.Route?> GetRouteByIdAsync(string id)
        {
            await Task.Delay(50);
            return _routes.FirstOrDefault(r => r.Id == id);
        }

        public async Task<List<Models.Route>> GetRoutesByDriverAsync(string driverId)
        {
            await Task.Delay(100);
            return _routes.Where(r => r.DriverId == driverId).ToList();
        }

        public async Task<Models.Route> CreateRouteAsync(Models.Route route)
        {
            await Task.Delay(50);
            route.Id = $"SR-2024-{_routes.Count + 1:D3}";
            route.CreatedAt = DateTime.Now;
            route.Status = RouteStatus.Pending;
            _routes.Add(route);
            return route;
        }

        public async Task<Models.Route> UpdateRouteAsync(Models.Route route)
        {
            await Task.Delay(50);
            var existingRoute = _routes.FirstOrDefault(r => r.Id == route.Id);
            if (existingRoute != null)
            {
                var index = _routes.IndexOf(existingRoute);
                _routes[index] = route;
            }
            return route;
        }

        public async Task<Models.Route> OptimizeRouteAsync(string routeId)
        {
            await Task.Delay(500); // Simulate optimization calculation
            var route = _routes.FirstOrDefault(r => r.Id == routeId);
            if (route != null)
            {
                route.OptimizationScore = 90 + _random.Next(10);
                route.EstimatedDistance *= 0.95; // Optimized route is 5% shorter
                route.EstimatedTime = TimeSpan.FromMinutes(route.EstimatedTime.TotalMinutes * 0.95);
            }
            return route!;
        }

        public async Task<int> GetRouteProgressAsync(string routeId)
        {
            await Task.Delay(50);
            var route = _routes.FirstOrDefault(r => r.Id == routeId);
            if (route == null) return 0;

            var completedStops = route.Stops.Count(s => s.Status == StopStatus.Completed);
            return route.Stops.Count > 0 ? (completedStops * 100) / route.Stops.Count : 0;
        }

        public async Task UpdateStopStatusAsync(string routeId, int stopIndex, StopStatus status)
        {
            await Task.Delay(50);
            var route = _routes.FirstOrDefault(r => r.Id == routeId);
            if (route != null && stopIndex >= 0 && stopIndex < route.Stops.Count)
            {
                route.Stops[stopIndex].Status = status;
                if (status == StopStatus.Completed)
                {
                    route.Stops[stopIndex].CompletedAt = DateTime.Now;
                }
                route.CurrentStopIndex = stopIndex;
            }
        }

        private List<Models.Route> GenerateMockRoutes()
        {
            var routes = new List<Models.Route>();

            for (int i = 0; i < 5; i++)
            {
                var route = new Models.Route
                {
                    Id = $"SR-2024-{i + 1:D3}",
                    DriverId = $"driver-{i + 1:D3}",
                    VehicleId = $"VEH-{100 + i}",
                    Status = i < 3 ? RouteStatus.InProgress : RouteStatus.Pending,
                    StartLocation = new Location
                    {
                        Address = "SafeRoute Warehouse, Midrand, Gauteng",
                        City = "Johannesburg",
                        State = "Gauteng",
                        Latitude = -25.9875,
                        Longitude = 28.1288
                    },
                    EndLocation = new Location
                    {
                        Address = "SafeRoute Warehouse, Midrand, Gauteng",
                        City = "Johannesburg",
                        State = "Gauteng",
                        Latitude = -25.9875,
                        Longitude = 28.1288
                    },
                    EstimatedDistance = 57.0 + _random.Next(32), // kilometres (converted from 35.5 + 20 miles)
                    EstimatedTime = TimeSpan.FromHours(4 + _random.Next(2)),
                    StartTime = DateTime.Today.AddHours(8),
                    OptimizationScore = 75 + _random.Next(20),
                    TrafficConditions = (TrafficCondition)_random.Next(0, 3),
                    CreatedAt = DateTime.Now.AddDays(-1),
                    CreatedBy = "dispatcher-001",
                    Stops = GenerateRouteStops(8 + _random.Next(5))
                };

                routes.Add(route);
            }

            return routes;
        }

        private List<RouteStop> GenerateRouteStops(int count)
        {
            var stops = new List<RouteStop>();
            var addresses = new[]
            {
                "123 Rivonia Road, Sandton, Gauteng",
                "456 Nelson Mandela Square, Sandton, Gauteng",
                "789 Jan Smuts Avenue, Rosebank, Gauteng",
                "321 Oxford Road, Rosebank, Gauteng",
                "654 William Nicol Drive, Fourways, Gauteng",
                "987 Witkoppen Road, Fourways, Gauteng",
                "147 Main Road, Bryanston, Gauteng",
                "258 Republic Road, Randburg, Gauteng"
            };

            for (int i = 0; i < count; i++)
            {
                stops.Add(new RouteStop
                {
                    Id = $"stop-{i + 1:D3}",
                    Sequence = i + 1,
                    Location = new Location
                    {
                        Address = addresses[i % addresses.Length],
                        City = "Johannesburg",
                        State = "Gauteng",
                        Latitude = -26.2041 + (_random.NextDouble() * 0.1),
                        Longitude = 28.0473 + (_random.NextDouble() * 0.1)
                    },
                    CustomerName = $"Customer {i + 1}",
                    ContactPhone = $"+27 {80 + i} {100 + i:D3} {1000 + i:D4}",
                    Status = i < 3 ? StopStatus.Completed : StopStatus.Pending,
                    EstimatedArrival = DateTime.Now.AddHours(i * 0.5),
                    PackageId = $"PKG-{1000 + i}"
                });
            }

            return stops;
        }
    }
}
