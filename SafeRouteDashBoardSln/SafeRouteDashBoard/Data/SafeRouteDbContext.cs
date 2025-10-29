using Microsoft.EntityFrameworkCore;
using SafeRouteDashBoard.Data.Entities;

namespace SafeRouteDashBoard.Data
{
    /// <summary>
    /// SafeRoute Database Context - Maps to exact SQL schema from screenshots
    /// </summary>
    public class SafeRouteDbContext : DbContext
    {
        public SafeRouteDbContext(DbContextOptions<SafeRouteDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Dispatcher> Dispatchers { get; set; }
        public DbSet<LocationUpdate> LocationUpdates { get; set; }
        public DbSet<Geofence> Geofences { get; set; }
        public DbSet<ZoneEntry> ZoneEntries { get; set; }
        public DbSet<PanicAlert> PanicAlerts { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentResponse> IncidentResponses { get; set; }
        public DbSet<ThreatDetection> ThreatDetections { get; set; }
        public DbSet<CameraRecording> CameraRecordings { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<RouteEntity> Routes { get; set; }
        public DbSet<RiskZone> RiskZones { get; set; }
        public DbSet<SafetyScore> SafetyScores { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<DeviceStatus> DeviceStatuses { get; set; }
        public DbSet<MonthlyReport> MonthlyReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore non-entity model classes
            modelBuilder.Ignore<Models.Location>();
            modelBuilder.Ignore<Models.Driver>();
            modelBuilder.Ignore<Models.Route>();
            modelBuilder.Ignore<Models.Alert>();
            modelBuilder.Ignore<Models.Analytics>();
            modelBuilder.Ignore<Models.SafetyMetrics>();
            modelBuilder.Ignore<Models.Notification>();
            modelBuilder.Ignore<Models.User>();
            modelBuilder.Ignore<Models.DashboardData>();

            // Configure composite keys
            modelBuilder.Entity<ZoneEntry>()
                .HasKey(ze => new { ze.DriverId, ze.GeofenceId });

            modelBuilder.Entity<IncidentResponse>()
                .HasKey(ir => new { ir.IncidentId, ir.DispatcherId });

            // Configure relationships
            ConfigureRelationships(modelBuilder);

            // Configure decimal precision for South African currency (ZAR)
            ConfigureDecimalPrecision(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // User -> Company (Many-to-One)
            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            // Driver -> User (One-to-Many from user)
            modelBuilder.Entity<Driver>()
                .HasOne(d => d.User)
                .WithMany(u => u.Drivers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict); // avoid cascade chain from Users -> Drivers -> many dependents

            // Dispatcher -> User (One-to-Many from user)
            modelBuilder.Entity<Dispatcher>()
                .HasOne(d => d.User)
                .WithMany(u => u.Dispatchers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // LocationUpdate -> Driver (Many-to-One)
            modelBuilder.Entity<LocationUpdate>()
                .HasOne(lu => lu.Driver)
                .WithMany(d => d.LocationUpdates)
                .HasForeignKey(lu => lu.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // ZoneEntry -> Driver (Many-to-One)
            modelBuilder.Entity<ZoneEntry>()
                .HasOne(ze => ze.Driver)
                .WithMany(d => d.ZoneEntries)
                .HasForeignKey(ze => ze.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // ZoneEntry -> Geofence (Many-to-One)
            modelBuilder.Entity<ZoneEntry>()
                .HasOne(ze => ze.Geofence)
                .WithMany(g => g.ZoneEntries)
                .HasForeignKey(ze => ze.GeofenceId)
                .OnDelete(DeleteBehavior.Restrict);

            // PanicAlert -> Driver (Many-to-One)
            modelBuilder.Entity<PanicAlert>()
                .HasOne(pa => pa.Driver)
                .WithMany(d => d.PanicAlerts)
                .HasForeignKey(pa => pa.DriverId)
                .OnDelete(DeleteBehavior.Restrict); // prevent multiple cascade paths

            // PanicAlert -> Dispatcher (Many-to-One, optional)
            modelBuilder.Entity<PanicAlert>()
                .HasOne(pa => pa.Dispatcher)
                .WithMany(d => d.AcknowledgedAlerts)
                .HasForeignKey(pa => pa.AcknowledgedByDispatcher)
                .OnDelete(DeleteBehavior.SetNull);

            // Incident -> Driver (Many-to-One)
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Driver)
                .WithMany(d => d.Incidents)
                .HasForeignKey(i => i.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // IncidentResponse -> Incident (Many-to-One)
            modelBuilder.Entity<IncidentResponse>()
                .HasOne(ir => ir.Incident)
                .WithMany(i => i.Responses)
                .HasForeignKey(ir => ir.IncidentId)
                .OnDelete(DeleteBehavior.Restrict);

            // IncidentResponse -> Dispatcher (Many-to-One)
            modelBuilder.Entity<IncidentResponse>()
                .HasOne(ir => ir.Dispatcher)
                .WithMany(d => d.IncidentResponses)
                .HasForeignKey(ir => ir.DispatcherId)
                .OnDelete(DeleteBehavior.Restrict);

            // CameraRecording -> Incident (Many-to-One)
            modelBuilder.Entity<CameraRecording>()
                .HasOne(cr => cr.Incident)
                .WithMany(i => i.CameraRecordings)
                .HasForeignKey(cr => cr.IncidentId)
                .OnDelete(DeleteBehavior.Restrict);

            // CameraRecording -> ThreatDetection (Many-to-One, optional)
            modelBuilder.Entity<CameraRecording>()
                .HasOne(cr => cr.ThreatDetection)
                .WithMany(td => td.CameraRecordings)
                .HasForeignKey(cr => cr.DetectionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Delivery -> Driver (Many-to-One)
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Driver)
                .WithMany(dr => dr.Deliveries)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // SafetyScore -> Driver (Many-to-One)
            modelBuilder.Entity<SafetyScore>()
                .HasOne(ss => ss.Driver)
                .WithMany(d => d.SafetyScores)
                .HasForeignKey(ss => ss.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // EmergencyContact -> Driver (Many-to-One)
            modelBuilder.Entity<EmergencyContact>()
                .HasOne(ec => ec.Driver)
                .WithMany(d => d.EmergencyContacts)
                .HasForeignKey(ec => ec.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // DeviceStatus -> Driver (Many-to-One)
            modelBuilder.Entity<DeviceStatus>()
                .HasOne(ds => ds.Driver)
                .WithMany(d => d.DeviceStatuses)
                .HasForeignKey(ds => ds.DriverId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureDecimalPrecision(ModelBuilder modelBuilder)
        {
            // GPS coordinates (high precision needed)
            modelBuilder.Entity<LocationUpdate>()
                .Property(l => l.Lat)
                .HasPrecision(10, 7);

            modelBuilder.Entity<LocationUpdate>()
                .Property(l => l.Lng)
                .HasPrecision(10, 7);

            // Confidence scores (0-100)
            modelBuilder.Entity<ThreatDetection>()
                .Property(t => t.ConfidenceScore)
                .HasPrecision(5, 2);

            // Safety scores (0-100)
            modelBuilder.Entity<SafetyScore>()
                .Property(s => s.OverallScore)
                .HasPrecision(5, 2);
        }
    }
}
