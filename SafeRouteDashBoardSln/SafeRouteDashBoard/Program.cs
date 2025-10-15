using SafeRouteDashBoard.Components;
using SafeRouteDashBoard.Hubs;
using SafeRouteDashBoard.Services;
using MudBlazor.Services;

namespace SafeRouteDashBoard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Add MudBlazor services
            builder.Services.AddMudServices();

            // Add SignalR
            builder.Services.AddSignalR();

            // Register application services
            builder.Services.AddScoped<IDriverService, DriverService>();
            builder.Services.AddScoped<IAlertService, AlertService>();
            builder.Services.AddScoped<IRouteService, RouteService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<ISafetyService, SafetyService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Map SignalR hub
            app.MapHub<DashboardHub>("/dashboardhub");

            app.Run();
        }
    }
}
