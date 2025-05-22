using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes.Limitators
{
    public class ApplicationLimitator
    {
        // Поля
        Owner _owner;
        Vehicle _vehicle;

        public ApplicationLimitator(Owner owner, Vehicle vehicle)
        {
            _owner = owner;
            _vehicle = vehicle;
        }

        public bool Check()
        {
            /*// Проверка на пустые поля
            if (_owner == null || _vehicle == null)
            {
                MessageHelper.MessageNullFields();
                return false;
            }
            // Проверка на то, что действующих заявок нет
            else if (_vehicle.Applications?.Any(r => r.ApplicationStatusId == 1 || r.ApplicationStatusId == 2) == true)
            {
                MessageHelper.MessageActiveApplication();
                return false;
            }
            // Проверка на то, что у владельца и авто нет действующих сертификатов
            else if (_vehicle.Applications?.Any(r => 
                r.Certificates?.Any(d => 
                d.IsActive == 1 && r.Vehicle.OwnerId == _owner.OwnerId) == true) == true)
            {
                MessageHelper.MessageActiveApplication();
                return false;
            }
            // Если ошибок нет, то возвращаем true
            else
            {
                return true;
            }*/
            return true;
        }
    }
}
