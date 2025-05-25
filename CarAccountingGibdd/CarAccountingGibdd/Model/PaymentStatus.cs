using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Model
{
    public partial class PaymentStatus
    {
        public int PaymentStatusId { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
