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

            // Add Database Context Factory (for Blazor Server long-lived components)
            builder.Services.AddDbContextFactory<SafeRouteDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("SafeRouteDb")));

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

            // Ensure database is created and seed data on startup
            using (var scope = app.Services.CreateScope())
            {
                var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SafeRouteDbContext>>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                
                try
                {
                    // Create context from factory
                    using var context = await contextFactory.CreateDbContextAsync();
                    
                    // Ensure database is created
                    logger.LogInformation("Ensuring database is created...");
                    await context.Database.EnsureCreatedAsync();
                    logger.LogInformation("Database ready.");

                    // OPTIONAL: Comment out the lines below to disable automatic seeding
                    // Uncomment to enable dummy data on first run
                    /*
                    var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeederService>();
                    await seeder.SeedDatabaseAsync();
                    */
                    
                    logger.LogInformation("Seeding disabled. Add real data via admin interface or API.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while creating/seeding the database");
                    throw;
                }
            }

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
