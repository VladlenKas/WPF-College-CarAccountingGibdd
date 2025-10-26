using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Model;

public class CombinedReport
{
    public DateOnly PeriodStart { get; set; }
    public DateOnly PeriodEnd { get; set; }

    // Количественные показатели
    public int TotalApplications { get; set; }
    public int ActiveCertificateApplications { get; set; }
    public int InspectionsCount { get; set; }
    public int RegisteredVehicles { get; set; }
    public int IssuedCertificates { get; set; }
    public int ViolationsCount { get; set; }
    public double AverageProcessingTimeDays { get; set; }

    // Процентные показатели
    public double PercentCertificatesFromVehicles { get; set; }
    public double PercentActiveCertApplications { get; set; }
    public double PercentCompletedApplications { get; set; } // Статус 7
    public double PercentViolationsFromInspections { get; set; }
    public double PercentApplicationsWithViolations { get; set; } // Статусы 5 и 6

    public static CombinedReport GetCombinedReport(DateOnly start, DateOnly end, GibddContext context)
    {
        // Преобразуем DateOnly в DateTime для сравнения с DateTime в базе
        DateTime startDateTime = start.ToDateTime(TimeOnly.MinValue);
        DateTime endDateTime = end.ToDateTime(TimeOnly.MaxValue);

        var applications = context.Applications
            .Where(a => a.DatetimeSupply >= startDateTime && a.DatetimeSupply <= endDateTime)
            .ToList();

        int totalApplications = applications.Count;
        int activeCertificateApplications = applications.Count(a => a.Certificates.Any(c => c.IsActive == 1));
        int inspectionsCount = context.Inspections.Count(i => i.DatetimePlanned >= startDateTime && i.DatetimePlanned <= endDateTime);
        int registeredVehicles = context.Vehicles.Count(v => v.Applications.Any(a => a.DatetimeSupply >= startDateTime && a.DatetimeSupply <= endDateTime));
        int issuedCertificates = applications.Count(a => a.ApplicationStatusId == 7);
        int violationsCount = applications.Count(a => a.ApplicationStatusId == 5 || a.ApplicationStatusId == 6);

        double averageProcessingTime = applications
            .Where(a => a.DatetimeConfirm.HasValue)
            .Select(a => (a.DatetimeConfirm.Value - a.DatetimeSupply).TotalDays)
            .DefaultIfEmpty(0)
            .Average();

        // Проценты
        double percentCertificatesFromVehicles = registeredVehicles > 0 ? (double)issuedCertificates / registeredVehicles * 100 : 0;
        double percentActiveCertApplications = totalApplications > 0 ? (double)activeCertificateApplications / totalApplications * 100 : 0;
        double percentCompletedApplications = totalApplications > 0 ? (double)issuedCertificates / totalApplications * 100 : 0;
        double percentViolationsFromInspections = inspectionsCount > 0 ? (double)violationsCount / inspectionsCount * 100 : 0;
        double percentApplicationsWithViolations = totalApplications > 0 ? (double)violationsCount / totalApplications * 100 : 0;

        return new CombinedReport
        {
            PeriodStart = start,
            PeriodEnd = end,
            TotalApplications = totalApplications,
            ActiveCertificateApplications = activeCertificateApplications,
            InspectionsCount = inspectionsCount,
            RegisteredVehicles = registeredVehicles,
            IssuedCertificates = issuedCertificates,
            ViolationsCount = violationsCount,
            AverageProcessingTimeDays = averageProcessingTime,
            PercentCertificatesFromVehicles = percentCertificatesFromVehicles,
            PercentActiveCertApplications = percentActiveCertApplications,
            PercentCompletedApplications = percentCompletedApplications,
            PercentViolationsFromInspections = percentViolationsFromInspections,
            PercentApplicationsWithViolations = percentApplicationsWithViolations
        };
    }
}

public class ReportItem
{
    public string Indicator { get; set; } = null!;
    public string Value { get; set; } = null!;
}


