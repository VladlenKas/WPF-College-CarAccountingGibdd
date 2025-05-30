using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes.Services
{
    public class InspectionService
    {
        // Начало осмотра
        public static void StartInspection(Inspection inspection)
        {
            inspection.StatusId = 2;
            inspection.Application.ApplicationStatusId = 4;

            App.DbContext.Update(inspection);
            App.DbContext.SaveChanges();
        }

        // Отклонение
        public static void Reject(Inspection inspection)
        {
            inspection.StatusId = 5;
            inspection.Application.ApplicationStatusId = 5;
            inspection.DatetimeCompleted = DateTime.Now;

            ViolationsInspection violationsInspection = new ViolationsInspection()
            {
                InspectionId = inspection.InspectionId,
                ViolationsId = 1
            };

            App.DbContext.Add(violationsInspection);
            App.DbContext.Update(inspection);
            App.DbContext.SaveChanges();
        }
    }
}
