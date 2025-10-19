using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeRouteDashBoard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMPANIES",
                columns: table => new
                {
                    company_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    registration_number = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPANIES", x => x.company_id);
                });

            migrationBuilder.CreateTable(
                name: "GEOFENCES",
                columns: table => new
                {
                    geofence_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    zone_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    polygon_coordinates = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GEOFENCES", x => x.geofence_id);
                });

            migrationBuilder.CreateTable(
                name: "MONTHLY_REPORTS",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    company_id = table.Column<int>(type: "INTEGER", nullable: false),
                    safety_metrics = table.Column<string>(type: "TEXT", nullable: false),
                    risk_analysis = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MONTHLY_REPORTS", x => x.report_id);
                });

            migrationBuilder.CreateTable(
                name: "NOTIFICATIONS",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    message = table.Column<string>(type: "TEXT", nullable: false),
                    priority = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOTIFICATIONS", x => x.notification_id);
                });

            migrationBuilder.CreateTable(
                name: "RISK_ZONES",
                columns: table => new
                {
                    zone_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    risk_level = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    boundary_coordinates = table.Column<string>(type: "TEXT", nullable: false),
                    incident_count = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_ZONES", x => x.zone_id);
                });

            migrationBuilder.CreateTable(
                name: "ROUTES",
                columns: table => new
                {
                    route_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    planned_waypoints = table.Column<string>(type: "TEXT", nullable: false),
                    actual_path = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROUTES", x => x.route_id);
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_LOGS",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    log_type = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_LOGS", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "THREAT_DETECTIONS",
                columns: table => new
                {
                    detection_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    threat_type = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    confidence_score = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THREAT_DETECTIONS", x => x.detection_id);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    company_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_USERS_COMPANIES_company_id",
                        column: x => x.company_id,
                        principalTable: "COMPANIES",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DISPATCHERS",
                columns: table => new
                {
                    dispatcher_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    assigned_drivers = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPATCHERS", x => x.dispatcher_id);
                    table.ForeignKey(
                        name: "FK_DISPATCHERS_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "USERS",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DRIVERS",
                columns: table => new
                {
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    current_status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DRIVERS", x => x.driver_id);
                    table.ForeignKey(
                        name: "FK_DRIVERS_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "USERS",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DELIVERIES",
                columns: table => new
                {
                    delivery_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    risk_level = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    completed_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DELIVERIES", x => x.delivery_id);
                    table.ForeignKey(
                        name: "FK_DELIVERIES_DRIVERS_driver_id",
                        column: x => x.driver_id,
                        principalTable: "DRIVERS",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DEVICE_STATUS",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    battery_level = table.Column<int>(type: "INTEGER", nullable: true),
                    gps_enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEVICE_STATUS", x => x.status_id);
                    table.ForeignKey(
                        name: "FK_DEVICE_STATUS_DRIVERS_driver_id",
                        column: x => x.driver_id,
                        principalTable: "DRIVERS",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EMERGENCY_CONTACTS",
                columns: table => new
                {
                    contact_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    relationship = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMERGENCY_CONTACTS", x => x.contact_id);
                    table.ForeignKey(
                        name: "FK_EMERGENCY_CONTACTS_DRIVERS_driver_id",
                        column: x => x.driver_id,
                        principalTable: "DRIVERS",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INCIDENTS",
                columns: table => new
                {
                    incident_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    alert_id = table.Column<string>(type: "TEXT", nullable: false),
                    severity = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    evidence_files = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INCIDENTS", x => x.incident_id);
                    table.ForeignKey(
                        name: "FK_INCIDENTS_DRIVERS_driver_id",
                        column: x => x.driver_id,
                        principalTable: "DRIVERS",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOCATION_UPDATES",
                columns: table => new
                {
                    location_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    lat = table.Column<decimal>(type: "TEXT", precision: 10, scale: 7, nullable: false),
                    lng = table.Column<decimal>(type: "TEXT", precision: 10, scale: 7, nullable: false),
                    timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCATION_UPDATES", x => x.location_id);
                    table.ForeignKey(
                        name: "FK_LOCATION_UPDATES_DRIVERS_driver_id",
                        column: x => x.driver_id,
                        principalTable: "DRIVERS",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PANIC_ALERTS",
                columns: table => new
                {
                    alert_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    alert_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    acknowledged_by_dispatcher = table.Column<int>(type: "INTEGER", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    resolved_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PANIC_ALERTS", x => x.alert_id);
                    table.ForeignKey(
                        name: "FK_PANIC_ALERTS_DISPATCHERS_acknowledged_by_dispatcher",
                        column: x => x.acknowledged_by_dispatcher,
                        principalTable: "DISPATCHERS",
                        principalColumn: "dispatcher_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PANIC_ALERTS_DRIVERS_driver_id",
                        column: x => x.driver_id,
                        principalTable: "DRIVERS",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SAFETY_SCORES",
                columns: table => new
                {
                    score_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    overall_score = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false),
                    recommendations = table.Column<string>(type: "TEXT", nullable: true),
                    calculated_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAFETY_SCORES", x => x.score_id);
                    table.ForeignKey(
                        name: "FK_SAFETY_SCORES_DRIVERS_driver_id",
                        column: x => x.driver_id,
                        principalTable: "DRIVERS",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZONE_ENTRIES",
                columns: table => new
                {
                    driver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    geofence_id = table.Column<int>(type: "INTEGER", nullable: false),
                    alert_triggered = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZONE_ENTRIES", x => new { x.driver_id, x.geofence_id });
                    table.ForeignKey(
                        name: "FK_ZONE_ENTRIES_DRIVERS_driver_id",
                        column: x => x.driver_id,
                        principalTable: "DRIVERS",
                        principalColumn: "driver_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZONE_ENTRIES_GEOFENCES_geofence_id",
                        column: x => x.geofence_id,
                        principalTable: "GEOFENCES",
                        principalColumn: "geofence_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CAMERA_RECORDINGS",
                columns: table => new
                {
                    recording_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    incident_id = table.Column<int>(type: "INTEGER", nullable: false),
                    detection_id = table.Column<int>(type: "INTEGER", nullable: true),
                    trigger_type = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    file_path = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAMERA_RECORDINGS", x => x.recording_id);
                    table.ForeignKey(
                        name: "FK_CAMERA_RECORDINGS_INCIDENTS_incident_id",
                        column: x => x.incident_id,
                        principalTable: "INCIDENTS",
                        principalColumn: "incident_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CAMERA_RECORDINGS_THREAT_DETECTIONS_detection_id",
                        column: x => x.detection_id,
                        principalTable: "THREAT_DETECTIONS",
                        principalColumn: "detection_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "INCIDENT_RESPONSES",
                columns: table => new
                {
                    incident_id = table.Column<int>(type: "INTEGER", nullable: false),
                    dispatcher_id = table.Column<int>(type: "INTEGER", nullable: false),
                    response_type = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INCIDENT_RESPONSES", x => new { x.incident_id, x.dispatcher_id });
                    table.ForeignKey(
                        name: "FK_INCIDENT_RESPONSES_DISPATCHERS_dispatcher_id",
                        column: x => x.dispatcher_id,
                        principalTable: "DISPATCHERS",
                        principalColumn: "dispatcher_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INCIDENT_RESPONSES_INCIDENTS_incident_id",
                        column: x => x.incident_id,
                        principalTable: "INCIDENTS",
                        principalColumn: "incident_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CAMERA_RECORDINGS_detection_id",
                table: "CAMERA_RECORDINGS",
                column: "detection_id");

            migrationBuilder.CreateIndex(
                name: "IX_CAMERA_RECORDINGS_incident_id",
                table: "CAMERA_RECORDINGS",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "IX_DELIVERIES_driver_id",
                table: "DELIVERIES",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_DEVICE_STATUS_driver_id",
                table: "DEVICE_STATUS",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_DISPATCHERS_user_id",
                table: "DISPATCHERS",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_DRIVERS_user_id",
                table: "DRIVERS",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_EMERGENCY_CONTACTS_driver_id",
                table: "EMERGENCY_CONTACTS",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_INCIDENT_RESPONSES_dispatcher_id",
                table: "INCIDENT_RESPONSES",
                column: "dispatcher_id");

            migrationBuilder.CreateIndex(
                name: "IX_INCIDENTS_driver_id",
                table: "INCIDENTS",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_LOCATION_UPDATES_driver_id",
                table: "LOCATION_UPDATES",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_PANIC_ALERTS_acknowledged_by_dispatcher",
                table: "PANIC_ALERTS",
                column: "acknowledged_by_dispatcher");

            migrationBuilder.CreateIndex(
                name: "IX_PANIC_ALERTS_driver_id",
                table: "PANIC_ALERTS",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_SAFETY_SCORES_driver_id",
                table: "SAFETY_SCORES",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_company_id",
                table: "USERS",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_ZONE_ENTRIES_geofence_id",
                table: "ZONE_ENTRIES",
                column: "geofence_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CAMERA_RECORDINGS");

            migrationBuilder.DropTable(
                name: "DELIVERIES");

            migrationBuilder.DropTable(
                name: "DEVICE_STATUS");

            migrationBuilder.DropTable(
                name: "EMERGENCY_CONTACTS");

            migrationBuilder.DropTable(
                name: "INCIDENT_RESPONSES");

            migrationBuilder.DropTable(
                name: "LOCATION_UPDATES");

            migrationBuilder.DropTable(
                name: "MONTHLY_REPORTS");

            migrationBuilder.DropTable(
                name: "NOTIFICATIONS");

            migrationBuilder.DropTable(
                name: "PANIC_ALERTS");

            migrationBuilder.DropTable(
                name: "RISK_ZONES");

            migrationBuilder.DropTable(
                name: "ROUTES");

            migrationBuilder.DropTable(
                name: "SAFETY_SCORES");

            migrationBuilder.DropTable(
                name: "SYSTEM_LOGS");

            migrationBuilder.DropTable(
                name: "ZONE_ENTRIES");

            migrationBuilder.DropTable(
                name: "THREAT_DETECTIONS");

            migrationBuilder.DropTable(
                name: "INCIDENTS");

            migrationBuilder.DropTable(
                name: "DISPATCHERS");

            migrationBuilder.DropTable(
                name: "GEOFENCES");

            migrationBuilder.DropTable(
                name: "DRIVERS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "COMPANIES");
        }
    }
}
