using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Application
{
    public int ApplicationId { get; set; }

    public int ApplicationStatusId { get; set; }

    public int? OperatorId { get; set; }

    public int OwnerId { get; set; }

    public int VehicleId { get; set; }

    public DateTime DatetimeSupply { get; set; }

    public DateTime? DatetimeConfirm { get; set; }

    public DateTime? DatetimeAccept { get; set; }

    public virtual ApplicationStatus ApplicationStatus { get; set; } = null!;

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    public virtual Employee? Operator { get; set; }

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Vehicle Vehicle { get; set; } = null!;

    public string OperatorFullname =>
        Operator?.Fullname ?? "Отсутствует";

    public string PaymentStatusName =>
        Payments?.FirstOrDefault()?.Status?.Name ?? "Отсутствует";

    public string PaymentStatusNumber =>
        Payments?.FirstOrDefault()?.PaymentId.ToString() ?? "Отсутствует";

    public string InspectorFullname =>
        Inspections?.FirstOrDefault()?.Inspector?.Fullname ?? "Отсутствует";

    public string InspectionNumber =>
        Inspections?.FirstOrDefault()?.InspectionId.ToString() ?? "Отсутствует";

}
