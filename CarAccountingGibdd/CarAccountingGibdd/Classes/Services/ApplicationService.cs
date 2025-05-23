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
        public void CreateApplication(Owner owner, Vehicle vehicle)
        {
            Application application = new()
            {
                OwnerId = owner.OwnerId,
                VehicleId = vehicle.VehicleId,
                ApplicationStatusId = 1, // Назначаем на осмотр
                DatetimeSupply = DateTime.Now
            };

            App.DbContext.Add(application);
        }

        // Проверка (ПРОВЕРИТЬ РАБОТУ!)
        public bool Check(Owner owner, Vehicle vehicle)
        {
            // Проверка на пустые поля
            if (owner == null || vehicle == null)
            {
                MessageHelper.MessageNullFields();
                return false;
            }
            // Проверка на то, что действующих заявок нет
            else if (vehicle.Applications?.Any(r => r.ApplicationStatusId == 1 || r.ApplicationStatusId == 2) == true)
            {
                MessageHelper.MessageActiveApplication();
                return false;
            }
            // Проверка на то, что у владельца и авто нет действующих сертификатов
            else if (App.DbContext.Applications.Any(r =>
                (r.Owner.OwnerId == owner.OwnerId && r.Vehicle.VehicleId == vehicle.VehicleId) && 
                r.ApplicationStatusId == 1 || r.ApplicationStatusId == 2))
            {
                MessageHelper.MessageActiveApplication();
                return false;
            }
            // Проверка на то, что у владельца и авто нет действующих сертификатов
            else if (App.DbContext.Certificates.Any(c =>
                c.IsActive == 1 && (c.OwnerVehicle.OwnerId == owner.OwnerId || c.OwnerVehicle.VehicleId == vehicle.VehicleId)))
            {
                MessageHelper.MessageActiveApplication();
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
