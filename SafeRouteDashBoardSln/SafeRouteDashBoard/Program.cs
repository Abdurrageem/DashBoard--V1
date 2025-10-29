using SafeRouteDashBoard.Components;
using SafeRouteDashBoard.Hubs;
using SafeRouteDashBoard.Services;
using SafeRouteDashBoard.Data;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;

namespace SafeRouteDashBoard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Add MudBlazor services
            builder.Services.AddMudServices();

            // Add SignalR
            builder.Services.AddSignalR();

            // Add Database Context Factory (use SQL Server -> reads conn string from appsettings or user secrets)
            builder.Services.AddDbContextFactory<SafeRouteDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SafeRouteDb")));

            // Register application services
            builder.Services.AddScoped<IDriverService, DriverService>();
            builder.Services.AddScoped<IAlertService, AlertService>();
            builder.Services.AddScoped<IRouteService, RouteService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<ISafetyService, SafetyService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IExportService, ExportService>();
            builder.Services.AddScoped<IDatabaseSeederService, DatabaseSeederService>();

            var app = builder.Build();

            // Ensure database is reachable/created and (optionally) seed data on startup
            using (var scope = app.Services.CreateScope())
            {
                var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SafeRouteDbContext>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                
                try
                {
                    // Create context from factory
                    using var context = await contextFactory.CreateDbContextAsync();

                    // Basic connectivity check & info (no sensitive data)
                    var dataSource = context.Database.GetDbConnection().DataSource;
                    var database = context.Database.GetDbConnection().Database;
                    logger.LogInformation("Attempting DB connection to '{DataSource}' / DB '{Database}'...", dataSource, database);
                    var canConnect = await context.Database.CanConnectAsync();
                    if (!canConnect)
                    {
                        logger.LogError("Database connectivity check failed. Verify connection string and firewall/IP settings.");
                    }

                    // Ensure database exists (for simple demo). Prefer Migrations in production.
                    await context.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database ready and reachable.");

                    // OPTIONAL: seeding disabled by default
                    // var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeederService>();
                    // await seeder.SeedDatabaseAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while connecting/initializing the database");
                    throw;
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
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
