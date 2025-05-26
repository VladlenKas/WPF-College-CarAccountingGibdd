using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes.Services
{
    public class ApplicationService
    {
        // Добавление
        public void CreateApplication(Owner owner, Vehicle vehicle, int paymentMethod, int change, string bank)
        {
            Application application = new()
            {
                OwnerId = owner.OwnerId,
                VehicleId = vehicle.VehicleId,
                ApplicationStatusId = 1, // На проверке документов
                DatetimeSupply = DateTime.Now
            };

            App.DbContext.Add(application);
            App.DbContext.SaveChanges();

            Payment payment = new Payment()
            {
                PaymentMethod = (sbyte)paymentMethod,
                BankName = bank,
                ApplicationId = application.ApplicationId,
                Amount = 400,
                PaymentDatetime = DateTime.Now,
                StatusId = 1,
            };

            App.DbContext.Add(payment);
            App.DbContext.SaveChanges();
        }

        // Проверка (ПРОВЕРИТЬ РАБОТУ!)
        public bool Check(Owner owner, Vehicle vehicle, int paymentMethod, int change, string bank)
        {
            // Проверка на пустые поля
            if (owner == null || vehicle == null ||
               (paymentMethod == 0 && bank == null))
            {
                MessageHelper.MessageNullFields();
                return false;
            }
            else if (paymentMethod == 1 && change < 0)
            {
                MessageHelper.MessageNullCost();
                return false;
            }
            // Проверка на то, что действующих заявок нет
            else if (App.DbContext.Applications.Any(r =>
                r.Vehicle.VehicleId == vehicle.VehicleId &&
                (r.ApplicationStatusId != 5 || r.ApplicationStatusId != 6)))
            {
                MessageHelper.MessageActiveApplication();
                return false;
            }
            // Проверка на то, что у владельца и авто уже нет сертификата
            else if (App.DbContext.Certificates.Any(r => 
                r.Application.OwnerId == owner.OwnerId && 
                r.Application.VehicleId == vehicle.VehicleId && 
                r.IsActive == 0))
            {
                MessageHelper.MessageCerrentSertificate();
                return false;
            }
            // Проверка на то, что у авто нет другого владельца
            else if (App.DbContext.Certificates.Any(r =>
                r.Application.VehicleId == vehicle.VehicleId &&
                r.IsActive == 0))
            {
                MessageHelper.MessageCerrentOwner();
                return false;
            }
            // Если ошибок нет, то возвращаем true
            else
            {
                return true;
            }
        }
    }
}
