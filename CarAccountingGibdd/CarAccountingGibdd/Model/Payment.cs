using System;
using System.Collections.Generic;

namespace CarAccountingGibdd.Model;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int ApplicationId { get; set; }

    public int StatusId { get; set; }

    public sbyte PaymentMethod { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDatetime { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual PaymentStatus Status { get; set; } = null!;
}
