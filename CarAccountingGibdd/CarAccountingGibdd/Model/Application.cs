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

    public DateTime? DatetimeAccept { get; set; }

    public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Vehicle Vehicle { get; set; } = null!;

    public string FirstEmployeeFullname =>
    Inspections?.FirstOrDefault()?.Employee?.Fullname ?? "Отсутствует";

    public string FirstInspectionNumber =>
        Inspections?.FirstOrDefault()?.InspectionId.ToString() ?? "Отсутствует";
}
