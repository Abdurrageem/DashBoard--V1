# ??? REAL DATA POPULATION GUIDE

## Overview
Your SafeRoute Dashboard currently uses:
1. **Database Seeder** - For initial dummy data (development)
2. **Mock Services** - DriverService, RouteService (in-memory data)
3. **Database Services** - DashboardService, AnalyticsService (real DB queries)

---

## ?? How to Populate with REAL Data

### Option 1: API Integration (Recommended for Production)

#### A. Create API Controllers
Create REST API endpoints to receive data from external sources (mobile apps, other systems):

```csharp
// Controllers/DriversController.cs
[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly SafeRouteDbContext _context;

    [HttpPost]
    public async Task<IActionResult> CreateDriver([FromBody] DriverDto dto)
    {
        var driver = new Driver
        {
            UserId = dto.UserId,
            CurrentStatus = dto.Status
        };
        
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();
        
        return Ok(driver);
    }

    [HttpPut("{id}/location")]
    public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationDto location)
    {
        var locationUpdate = new LocationUpdate
        {
            DriverId = id,
            Lat = location.Latitude,
            Lng = location.Longitude,
            Timestamp = DateTime.Now
        };
        
        await _context.LocationUpdates.AddAsync(locationUpdate);
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPost("{id}/panic-alert")]
    public async Task<IActionResult> TriggerPanicAlert(int id, [FromBody] PanicAlertDto alert)
    {
        var panicAlert = new PanicAlert
        {
            DriverId = id,
            AlertType = alert.Type,
            Status = "Active",
            CreatedAt = DateTime.Now
        };
        
        await _context.PanicAlerts.AddAsync(panicAlert);
        await _context.SaveChangesAsync();
        
        // Send real-time notification via SignalR
        await _hubContext.Clients.All.SendAsync("PanicAlertReceived", panicAlert);
        
        return Ok();
    }
}
```

#### B. Mobile App Integration
Your mobile driver app would call these endpoints:

```csharp
// Mobile App Code
public async Task SendLocationUpdate()
{
    var location = new LocationDto
    {
        Latitude = _gpsService.Latitude,
        Longitude = _gpsService.Longitude
    };
    
    await _httpClient.PutAsJsonAsync($"api/drivers/{driverId}/location", location);
}

public async Task TriggerPanicButton()
{
    var alert = new PanicAlertDto
    {
        Type = "Panic",
        Location = _gpsService.CurrentLocation
    };
    
    await _httpClient.PostAsJsonAsync($"api/drivers/{driverId}/panic-alert", alert);
}
```

---

### Option 2: Manual Data Entry (Admin Interface)

#### Create Admin Forms
Add admin pages to manually enter data:

```razor
@* Pages/Admin/AddDriver.razor *@
@page "/admin/drivers/add"

<MudForm @ref="form" @bind-IsValid="@success">
    <MudTextField T="string" @bind-Value="model.Name" Label="Driver Name" Required="true" />
    <MudTextField T="string" @bind-Value="model.Phone" Label="Phone Number" Required="true" />
    <MudSelect T="string" @bind-Value="model.Status" Label="Status">
        <MudSelectItem Value="@("Active")">Active</MudSelectItem>
        <MudSelectItem Value="@("OnBreak")">On Break</MudSelectItem>
        <MudSelectItem Value="@("Offline")">Offline</MudSelectItem>
    </MudSelect>
    
    <MudButton Variant="Variant.Filled" Color="Color.Primary" OnClick="Submit">
        Add Driver
    </MudButton>
</MudForm>

@code {
    private async Task Submit()
    {
        // Create user first
        var user = new UserEntity
        {
            Role = "Driver",
            CompanyId = 1
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Create driver
        var driver = new Driver
        {
            UserId = user.UserId,
            CurrentStatus = model.Status
        };
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();
        
        Snackbar.Add("Driver added successfully!", Severity.Success);
    }
}
```

---

### Option 3: CSV Import

#### Create Import Service
```csharp
public class CsvImportService
{
    private readonly SafeRouteDbContext _context;

    public async Task ImportDriversFromCsv(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<DriverCsvRow>();
        
        foreach (var record in records)
        {
            var user = new UserEntity
            {
                Role = "Driver",
                CompanyId = 1
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var driver = new Driver
            {
                UserId = user.UserId,
                CurrentStatus = record.Status
            };
            await _context.Drivers.AddAsync(driver);
        }
        
        await _context.SaveChangesAsync();
    }
}
```

#### CSV Format Example:
```csv
Name,Phone,Status,LicenseNumber
John Doe,+27821234567,Active,DL123456
Jane Smith,+27829876543,OnBreak,DL789012
```

---

### Option 4: Database Migration from Existing System

If you have existing data in another database:

```csharp
public class DataMigrationService
{
    private readonly SafeRouteDbContext _newContext;
    private readonly OldSystemDbContext _oldContext;

    public async Task MigrateDrivers()
    {
        var oldDrivers = await _oldContext.Drivers.ToListAsync();
        
        foreach (var oldDriver in oldDrivers)
        {
            var user = new UserEntity
            {
                Role = "Driver",
                CompanyId = 1
            };
            await _newContext.Users.AddAsync(user);
            await _newContext.SaveChangesAsync();

            var newDriver = new Driver
            {
                UserId = user.UserId,
                CurrentStatus = oldDriver.Status
            };
            await _newContext.Drivers.AddAsync(newDriver);
        }
        
        await _newContext.SaveChangesAsync();
    }
}
```

---

## ?? How the System Works Now

### Current Architecture:

#### 1. **Database Layer** (Real Data - SQLite)
```
SafeRouteDbContext
??? Companies (2 records)
??? Users (7 records)
??? Drivers (5 records)
??? Deliveries (13 records)
??? PanicAlerts (9 records)
??? RiskZones (4 records)
??? SafetyScores (10 records)
??? LocationUpdates (21 records)
```

**Used by:**
- ? DashboardService (GetKpiMetricsAsync)
- ? AnalyticsService (GetDashboardKpiMetricsAsync, GetPanicAlertTrendsAsync)
- ? ExportService (All CSV exports)

#### 2. **Mock Services** (In-Memory Data)
```
DriverService.cs - Generates 5 fake drivers
RouteService.cs - Generates 5 fake routes
AlertService.cs - Generates 3 fake alerts
SafetyService.cs - Generates fake metrics
NotificationService.cs - Generates 3 fake notifications
```

**Problem:** These services create data on-the-fly instead of reading from database.

---

## ?? To Make Everything Use REAL Data

### Step 1: Update DriverService to use Database

**Current (Mock):**
```csharp
public class DriverService : IDriverService
{
    private readonly List<Driver> _drivers;
    
    public DriverService()
    {
        _drivers = GenerateMockDrivers(); // ? Generates fake data
    }
}
```

**Change to (Real DB):**
```csharp
public class DriverService : IDriverService
{
    private readonly SafeRouteDbContext _context;
    
    public DriverService(SafeRouteDbContext context)
    {
        _context = context;
    }

    public async Task<List<Models.Driver>> GetActiveDriversAsync()
    {
        var dbDrivers = await _context.Drivers
            .Include(d => d.User)
            .Where(d => d.CurrentStatus != "Offline")
            .ToListAsync();

        // Map to DTO
        return dbDrivers.Select(d => new Models.Driver
        {
            Id = d.DriverId.ToString(),
            Name = "Driver " + d.DriverId, // Get from User table
            Status = Enum.Parse<DriverStatus>(d.CurrentStatus),
            // ... map other properties
        }).ToList();
    }
}
```

### Step 2: Add Data Entry Pages

Create these admin pages:

#### A. Add Driver Page
```
/admin/drivers/add
- Form to add new driver
- Saves to database
```

#### B. Add Delivery Page
```
/admin/deliveries/add
- Form to create delivery
- Assign to driver
- Set status, risk level
```

#### C. Trigger Panic Alert
```
/admin/alerts/create
- Simulate panic button
- Select driver
- Set alert type
```

---

## ?? Quick Data Entry Methods

### Method 1: SQL Inserts (Fastest)

Use DB Browser for SQLite or SQL commands:

```sql
-- Add a new driver
INSERT INTO USERS (role, company_id) VALUES ('Driver', 1);
INSERT INTO DRIVERS (user_id, current_status) VALUES (last_insert_rowid(), 'Active');

-- Add a delivery
INSERT INTO DELIVERIES (driver_id, risk_level, status, created_at) 
VALUES (1, 'Low', 'InProgress', datetime('now'));

-- Add panic alert
INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, created_at) 
VALUES (1, 'Panic', 'Active', datetime('now'));

-- Update driver status
UPDATE DRIVERS SET current_status = 'OnBreak' WHERE driver_id = 1;
```

### Method 2: C# Code (In Program.cs or Service)

```csharp
// Add after seeding in Program.cs
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SafeRouteDbContext>();
    
    // Add a new driver
    var user = new UserEntity { Role = "Driver", CompanyId = 1 };
    await context.Users.AddAsync(user);
    await context.SaveChangesAsync();
    
    var driver = new Driver { UserId = user.UserId, CurrentStatus = "Active" };
    await context.Drivers.AddAsync(driver);
    await context.SaveChangesAsync();
}
```

### Method 3: HTTP Requests (If you add API endpoints)

```bash
# Add driver
curl -X POST https://localhost:5001/api/drivers \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","status":"Active"}'

# Update location
curl -X PUT https://localhost:5001/api/drivers/1/location \
  -H "Content-Type: application/json" \
  -d '{"latitude":-26.2041,"longitude":28.0473}'

# Trigger panic alert
curl -X POST https://localhost:5001/api/drivers/1/panic-alert \
  -H "Content-Type: application/json" \
  -d '{"type":"Panic"}'
```

---

## ?? Recommended Approach for Production

### 1. **Phase 1: API Layer**
Create REST API controllers for:
- Drivers (CRUD operations)
- Deliveries (Create, Update, Complete)
- Panic Alerts (Create, Acknowledge, Resolve)
- Location Updates (Real-time tracking)

### 2. **Phase 2: Mobile Integration**
Connect mobile driver app to API:
- Send location every 30 seconds
- Track delivery status
- Panic button triggers alert
- Update driver status

### 3. **Phase 3: Admin Interface**
Build admin pages:
- Add/Edit drivers
- Manage deliveries
- View panic alerts
- Configure risk zones

### 4. **Phase 4: Real-Time Updates**
Use SignalR for:
- Live location updates
- Instant panic alert notifications
- Dashboard auto-refresh

---

## ?? Sample Real Data Flow

### Scenario: Driver Completes Delivery

#### 1. Mobile App Sends Request:
```csharp
POST /api/deliveries/123/complete
{
    "driverId": 1,
    "completedAt": "2025-01-19T14:30:00",
    "signature": "base64_image_data"
}
```

#### 2. API Updates Database:
```csharp
var delivery = await _context.Deliveries.FindAsync(123);
delivery.Status = "Completed";
delivery.CompletedAt = DateTime.Now;
await _context.SaveChangesAsync();
```

#### 3. SignalR Notifies Dashboard:
```csharp
await _hubContext.Clients.All.SendAsync("DeliveryCompleted", delivery);
```

#### 4. Dashboard Updates in Real-Time:
```csharp
// AnalyticsReal.razor
hubConnection.On<Delivery>("DeliveryCompleted", (delivery) => {
    // Refresh charts
    await LoadDataAsync();
    StateHasChanged();
});
```

---

## ?? How to View Your Current Data

### Option 1: DB Browser for SQLite
```
1. Download: https://sqlitebrowser.org/
2. Open: SafeRoute.db
3. Browse Data tab
4. View/Edit tables
```

### Option 2: SQL Queries
```sql
-- View all drivers
SELECT * FROM DRIVERS;

-- View today's deliveries
SELECT * FROM DELIVERIES 
WHERE date(created_at) = date('now');

-- View active alerts
SELECT * FROM PANIC_ALERTS 
WHERE status = 'Active';

-- View driver with most deliveries
SELECT driver_id, COUNT(*) as delivery_count
FROM DELIVERIES
GROUP BY driver_id
ORDER BY delivery_count DESC;
```

### Option 3: Add Debug Page
```razor
@page "/admin/debug"

<MudPaper Class="pa-4">
    <MudText Typo="Typo.h5">Database Stats</MudText>
    
    <MudSimpleTable>
        <tbody>
            <tr>
                <td>Companies</td>
                <td>@companies</td>
            </tr>
            <tr>
                <td>Users</td>
                <td>@users</td>
            </tr>
            <tr>
                <td>Drivers</td>
                <td>@drivers</td>
            </tr>
            <tr>
                <td>Deliveries</td>
                <td>@deliveries</td>
            </tr>
        </tbody>
    </MudSimpleTable>
</MudPaper>

@code {
    private int companies, users, drivers, deliveries;

    protected override async Task OnInitializedAsync()
    {
        companies = await _context.Companies.CountAsync();
        users = await _context.Users.CountAsync();
        drivers = await _context.Drivers.CountAsync();
        deliveries = await _context.Deliveries.CountAsync();
    }
}
```

---

## ?? Next Steps to Get Real Data

### Option A: Build API + Mobile App (Best for production)
1. Create API controllers
2. Build mobile app for drivers
3. Integrate real-time tracking
4. Deploy and use

### Option B: Admin Interface (Quick start)
1. Create admin pages for data entry
2. Manually add drivers, deliveries
3. Test workflow
4. Expand as needed

### Option C: Import Existing Data (If you have data)
1. Export from old system
2. Create import service
3. Run migration
4. Verify data

---

## ?? Summary

**Current State:**
- ? Database structure ready (21 tables)
- ? Seeder adds sample data (85 records)
- ? Some services use real DB (DashboardService, AnalyticsService)
- ? Some services use mock data (DriverService, RouteService)

**To Get Real Data:**
1. **Immediate:** Add SQL inserts or use DB Browser
2. **Short-term:** Build admin interface for manual entry
3. **Long-term:** Build API + mobile integration

**The database IS working and ready for real data!**
You just need to choose how you want to add it. ??
