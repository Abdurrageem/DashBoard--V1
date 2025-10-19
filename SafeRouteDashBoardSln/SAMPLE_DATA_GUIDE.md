# ??? How to Add Sample Data to SafeRoute Database

## Quick SQL Script for Sample Data

Copy and paste this SQL into DB Browser for SQLite:

```sql
-- 1. Add Companies
INSERT INTO COMPANIES (company_id, registration_number) VALUES (1, '2023/123456/07');
INSERT INTO COMPANIES (company_id, registration_number) VALUES (2, '2022/654321/07');

-- 2. Add Users
INSERT INTO USERS (user_id, role, company_id) VALUES (1, 'Admin', 1);
INSERT INTO USERS (user_id, role, company_id) VALUES (2, 'Dispatcher', 1);
INSERT INTO USERS (user_id, role, company_id) VALUES (3, 'Driver', 1);
INSERT INTO USERS (user_id, role, company_id) VALUES (4, 'Driver', 1);
INSERT INTO USERS (user_id, role, company_id) VALUES (5, 'Driver', 1);

-- 3. Add Drivers
INSERT INTO DRIVERS (driver_id, user_id, current_status) VALUES (1, 3, 'Active');
INSERT INTO DRIVERS (driver_id, user_id, current_status) VALUES (2, 4, 'Active');
INSERT INTO DRIVERS (driver_id, user_id, current_status) VALUES (3, 5, 'OnBreak');

-- 4. Add Dispatcher
INSERT INTO DISPATCHERS (dispatcher_id, user_id, assigned_drivers) VALUES (1, 2, '1,2,3');

-- 5. Add Deliveries (for today's metrics)
INSERT INTO DELIVERIES (driver_id, risk_level, status, created_at, completed_at) 
VALUES (1, 'Low', 'Completed', datetime('now'), datetime('now', '+2 hours'));

INSERT INTO DELIVERIES (driver_id, risk_level, status, created_at) 
VALUES (2, 'Medium', 'InProgress', datetime('now'));

INSERT INTO DELIVERIES (driver_id, risk_level, status, created_at, completed_at) 
VALUES (1, 'Low', 'Completed', datetime('now', '-1 hour'), datetime('now'));

INSERT INTO DELIVERIES (driver_id, risk_level, status, created_at, completed_at) 
VALUES (2, 'Low', 'Completed', datetime('now', '-2 hours'), datetime('now', '-1 hour'));

INSERT INTO DELIVERIES (driver_id, risk_level, status, created_at, completed_at) 
VALUES (3, 'High', 'Completed', datetime('now', '-3 hours'), datetime('now', '-1 hour'));

-- 6. Add Panic Alerts (for testing)
INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, created_at) 
VALUES (1, 'Panic', 'Active', datetime('now', '-10 minutes'));

INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, acknowledged_by_dispatcher, created_at, resolved_at) 
VALUES (2, 'Accident', 'Resolved', 1, datetime('now', '-2 hours'), datetime('now', '-1 hour'));

INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, created_at) 
VALUES (1, 'Hijack', 'Active', datetime('now', '-5 minutes'));

-- 7. Add Risk Zones
INSERT INTO RISK_ZONES (risk_level, boundary_coordinates, incident_count) 
VALUES ('High', '[{lat:-26.2041,lng:28.0473}]', 15);

INSERT INTO RISK_ZONES (risk_level, boundary_coordinates, incident_count) 
VALUES ('Medium', '[{lat:-26.1500,lng:28.0300}]', 5);

INSERT INTO RISK_ZONES (risk_level, boundary_coordinates, incident_count) 
VALUES ('Critical', '[{lat:-26.3500,lng:28.1500}]', 42);

INSERT INTO RISK_ZONES (risk_level, boundary_coordinates, incident_count) 
VALUES ('Low', '[{lat:-26.0500,lng:28.0200}]', 1);

-- 8. Add Safety Scores
INSERT INTO SAFETY_SCORES (driver_id, overall_score, recommendations, calculated_at) 
VALUES (1, 92.5, 'Excellent safety record. Continue current practices.', datetime('now'));

INSERT INTO SAFETY_SCORES (driver_id, overall_score, recommendations, calculated_at) 
VALUES (2, 78.3, 'Good performance. Consider defensive driving training.', datetime('now'));

INSERT INTO SAFETY_SCORES (driver_id, overall_score, recommendations, calculated_at) 
VALUES (3, 85.7, 'Very good. Maintain awareness in high-risk zones.', datetime('now'));

-- 9. Add Location Updates (last 7 days for charts)
INSERT INTO LOCATION_UPDATES (driver_id, lat, lng, timestamp) 
VALUES (1, -26.2041, 28.0473, datetime('now', '-1 day'));

INSERT INTO LOCATION_UPDATES (driver_id, lat, lng, timestamp) 
VALUES (2, -26.1500, 28.0300, datetime('now', '-2 days'));

INSERT INTO LOCATION_UPDATES (driver_id, lat, lng, timestamp) 
VALUES (1, -26.2100, 28.0500, datetime('now', '-3 days'));

-- 10. Add more Panic Alerts for trend chart (last 7 days)
INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, created_at) 
VALUES (1, 'Panic', 'Resolved', datetime('now', '-1 day'));

INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, created_at) 
VALUES (2, 'Medical', 'Resolved', datetime('now', '-2 days'));

INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, created_at) 
VALUES (1, 'Accident', 'Resolved', datetime('now', '-3 days'));

INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, created_at) 
VALUES (2, 'Hijack', 'Resolved', datetime('now', '-4 days'));

INSERT INTO PANIC_ALERTS (driver_id, alert_type, status, created_at) 
VALUES (3, 'Panic', 'Resolved', datetime('now', '-5 days'));
```

---

## Using DB Browser for SQLite

### Step 1: Download & Install
- Go to: https://sqlitebrowser.org/
- Download for Windows
- Install the application

### Step 2: Open Database
1. Open DB Browser for SQLite
2. Click "Open Database"
3. Navigate to your project folder
4. Select `SafeRoute.db`

### Step 3: Execute SQL
1. Click "Execute SQL" tab
2. Paste the SQL script above
3. Click "Execute" button (? icon)
4. Click "Write Changes" to save

### Step 4: Verify Data
1. Click "Browse Data" tab
2. Select a table from dropdown
3. View inserted records

---

## Expected Results

After running the script:

### Dashboard KPIs:
- **Active Drivers:** 2
- **Open Panic Alerts:** 2
- **Average Safety Score:** 85.5
- **High-Risk Zones:** 2

### Analytics Charts:
- **Panic Alerts Trend:** Shows 8 alerts over 7 days
- **Risk Distribution:** 4 zones (Critical, High, Medium, Low)
- **Driver Safety Scores:** 3 drivers with scores

### Deliveries:
- **Total Today:** 5
- **Completed:** 4
- **In Progress:** 1
- **On-Time %:** 80%

---

## Alternative: Manual Entry

You can also add data manually using DB Browser:

1. Click "Browse Data" tab
2. Select table (e.g., DRIVERS)
3. Click "Insert Record" button
4. Fill in the fields:
   - `driver_id`: 1
   - `user_id`: 3
   - `current_status`: Active
5. Click "Write Changes"
6. Repeat for other tables

---

## Refresh Application

After adding data:

1. Stop the running application (Ctrl+C)
2. Run again: `dotnet run`
3. Navigate to https://localhost:5001
4. You should see real data in charts and KPIs

---

## Troubleshooting

### If charts are empty:
- Make sure PANIC_ALERTS table has records with dates in the last 7 days
- Check created_at timestamps are correct

### If KPIs show 0:
- Verify DRIVERS table has records with current_status = 'Active'
- Check DELIVERIES table has records with created_at = today
- Ensure SAFETY_SCORES table has records

### If export doesn't work:
- Make sure you have data in the respective tables
- Check browser console for JavaScript errors

---

## Sample Data Included:
? 2 Companies  
? 5 Users (1 Admin, 1 Dispatcher, 3 Drivers)  
? 3 Drivers (2 Active, 1 On Break)  
? 1 Dispatcher  
? 5 Deliveries (4 Completed, 1 In Progress)  
? 8 Panic Alerts (2 Active, 6 Resolved)  
? 4 Risk Zones  
? 3 Safety Scores  
? 3 Location Updates  

---

**Your dashboard should now show real data! ???**
