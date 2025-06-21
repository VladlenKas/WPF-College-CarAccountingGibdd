using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Model
{
    public class Report
    {
        public int ApplcationId { get; set; }

        public string StatusName { get; set; } = null!;

        public string OwnerFullname { get; set; } = null!;

        public string VehicleFullInfo { get; set; } = null!;

        public string DepartmentName { get; set; } = null!;

        public DateTime DatetimeSupply { get; set; }

        public DateTime? DatetimeConfirm { get; set; }

        public string DatetimeConfirmValue =>
            DatetimeConfirm.HasValue
                ? DatetimeConfirm.Value.ToString("dd.MM.yyyy HH:mm")
                : "Не принята";
    }
}