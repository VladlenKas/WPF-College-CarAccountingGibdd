using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Application
{
    public int ApplicationId { get; set; }

    public int ApplicationStatusId { get; set; }

    public int OwnerId { get; set; }

    public int VehicleId { get; set; }

    public DateTime DatetimeSupply { get; set; }

    public DateTime? DatetimeConfirm { get; set; }

    public decimal Amount { get; set; }

    public sbyte PaymentMethod { get; set; }

    public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;

    public string InspectorFullname =>
        Inspections?.FirstOrDefault()?.Inspector?.Fullname ?? "Отсутствует";

    public string DepartmentName =>
        Inspections?.FirstOrDefault()?.Inspector?.Department.Name ?? "Отсутствует";

    public int? InspectorId =>
        Inspections?.FirstOrDefault()?.Inspector?.EmployeeId ?? null;

    public string InspectionNumber =>
        Inspections?.FirstOrDefault()?.InspectionId.ToString() ?? "Отсутствует";
    
    public string InspectionDate =>
        Inspections?.FirstOrDefault()?.DatetimePlanned.ToString() ?? "Отсутствует";

}
