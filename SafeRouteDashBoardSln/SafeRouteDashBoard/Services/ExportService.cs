using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using SafeRouteDashBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace SafeRouteDashBoard.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportPanicAlertsToCsvAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<byte[]> ExportDriverPerformanceToCsvAsync();
        Task<byte[]> ExportRiskZonesToCsvAsync();
        Task<byte[]> ExportDeliveriesToCsvAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<byte[]> ExportSafetyScoresToCsvAsync();
    }

    public class ExportService : IExportService
    {
        private readonly SafeRouteDbContext _context;

        public ExportService(SafeRouteDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> ExportPanicAlertsToCsvAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var alerts = await _context.PanicAlerts
                .Include(pa => pa.Driver)
                    .ThenInclude(d => d.User)
                .Where(pa => pa.CreatedAt >= startDate && pa.CreatedAt <= endDate)
                .OrderByDescending(pa => pa.CreatedAt)
                .Select(pa => new
                {
                    AlertId = pa.AlertId,
                    DriverId = pa.DriverId,
                    AlertType = pa.AlertType,
                    Status = pa.Status,
                    CreatedAt = pa.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
                    ResolvedAt = pa.ResolvedAt.HasValue ? pa.ResolvedAt.Value.ToString("dd/MM/yyyy HH:mm") : "N/A",
                    AcknowledgedBy = pa.AcknowledgedByDispatcher.HasValue ? pa.AcknowledgedByDispatcher.Value.ToString() : "Not Acknowledged"
                })
                .ToListAsync();

            return GenerateCsv(alerts);
        }

        public async Task<byte[]> ExportDriverPerformanceToCsvAsync()
        {
            var performance = await _context.SafetyScores
                .Include(ss => ss.Driver)
                    .ThenInclude(d => d.User)
                .OrderByDescending(ss => ss.OverallScore)
                .Select(ss => new
                {
                    DriverId = ss.DriverId,
                    OverallScore = ss.OverallScore.ToString("F2"),
                    CalculatedAt = ss.CalculatedAt.ToString("dd/MM/yyyy HH:mm"),
                    Recommendations = ss.Recommendations ?? "None"
                })
                .ToListAsync();

            return GenerateCsv(performance);
        }

        public async Task<byte[]> ExportRiskZonesToCsvAsync()
        {
            var zones = await _context.RiskZones
                .OrderByDescending(rz => rz.IncidentCount)
                .Select(rz => new
                {
                    ZoneId = rz.ZoneId,
                    RiskLevel = rz.RiskLevel,
                    IncidentCount = rz.IncidentCount ?? 0,
                    BoundaryCoordinates = rz.BoundaryCoordinates
                })
                .ToListAsync();

            return GenerateCsv(zones);
        }

        public async Task<byte[]> ExportDeliveriesToCsvAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate ??= DateTime.Today.AddDays(-30);
            endDate ??= DateTime.Today;

            var deliveries = await _context.Deliveries
                .Include(d => d.Driver)
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new
                {
                    DeliveryId = d.DeliveryId,
                    DriverId = d.DriverId,
                    Status = d.Status,
                    RiskLevel = d.RiskLevel,
                    CreatedAt = d.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
                    CompletedAt = d.CompletedAt.HasValue ? d.CompletedAt.Value.ToString("dd/MM/yyyy HH:mm") : "In Progress"
                })
                .ToListAsync();

            return GenerateCsv(deliveries);
        }

        public async Task<byte[]> ExportSafetyScoresToCsvAsync()
        {
            var scores = await _context.SafetyScores
                .Include(ss => ss.Driver)
                .OrderBy(ss => ss.DriverId)
                .ThenByDescending(ss => ss.CalculatedAt)
                .Select(ss => new
                {
                    ScoreId = ss.ScoreId,
                    DriverId = ss.DriverId,
                    OverallScore = ss.OverallScore.ToString("F2"),
                    CalculatedAt = ss.CalculatedAt.ToString("dd/MM/yyyy HH:mm"),
                    Recommendations = ss.Recommendations ?? "None"
                })
                .ToListAsync();

            return GenerateCsv(scores);
        }

        private byte[] GenerateCsv<T>(IEnumerable<T> records)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.UTF8
            });

            csvWriter.WriteRecords(records);
            streamWriter.Flush();

            return memoryStream.ToArray();
        }
    }
}
