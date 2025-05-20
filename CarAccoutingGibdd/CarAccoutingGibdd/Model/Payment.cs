using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarAccoutingGibdd.Model;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int ApplicationId { get; set; }

    public byte PaymentMethod { get; set; } = 0; // 0=наличные, 1=безнал

    public string? BankName { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDatetime { get; set; }

    public virtual Application Application { get; set; } = null!;
}
