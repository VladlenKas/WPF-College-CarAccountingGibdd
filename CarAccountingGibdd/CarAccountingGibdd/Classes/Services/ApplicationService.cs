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
        private readonly Owner _owner;
        private readonly Vehicle _vehicle;
        private readonly int _paymentMethod;
        private readonly int _change;
        private readonly string? _bank;

        public ApplicationService(Owner owner, Vehicle vehicle, int paymentMethod, int change, string? bank)
        {
            _owner = owner;
            _vehicle = vehicle;
            _paymentMethod = paymentMethod;
            _change = change;
            _bank = bank;
        }

        // Добавление
        public void CreateApplication()
        {
            Application application = new()
            {
                OwnerId = _owner.OwnerId,
                VehicleId = _vehicle.VehicleId,
                ApplicationStatusId = 1, // На проверке документов
                DatetimeSupply = DateTime.Now
            };

            App.DbContext.Add(application);
            App.DbContext.SaveChanges();

            Payment payment = new()
            {
                PaymentMethod = (sbyte)_paymentMethod,
                BankName = _bank,
                ApplicationId = application.ApplicationId,
                Amount = 400,
                PaymentDatetime = DateTime.Now,
                StatusId = 1,
            };

            App.DbContext.Add(payment);
            App.DbContext.SaveChanges();
        }

        // Проверка (добавить редактирование, ПРОВЕРИТЬ РАБОТУ!)
        public bool Check(Application? application = null)
        {
            // Проверки на пустые поля
            bool nullFields = _owner == null || _vehicle == null || _paymentMethod == -1 || (_paymentMethod == 0 && _bank == null);
            bool nullPay = _paymentMethod == 1 && _change < 0;

            if (nullFields)
            {
                MessageHelper.MessageNullFields();
                return false;
            }

            if (nullPay)
            {
                MessageHelper.MessageNullCost();
                return false;
            }

            // Проверка на то, что действующих заявок нет
            bool hasActiveApplication = App.DbContext.Applications.Any(r =>
                (r.Vehicle.VehicleId == _vehicle.VehicleId) &&
                (r.ApplicationStatusId != 5 || r.ApplicationStatusId != 6) &&
                (application == null || r.ApplicationId != application.ApplicationId));

            if (hasActiveApplication)
            {
                MessageHelper.MessageActiveApplication();
                return false;
            }

            // Проверка на то, что у владельца и авто уже нет сертификата
            bool hasCertificate = App.DbContext.Certificates.Any(r =>
                r.Application.OwnerId == _owner.OwnerId &&
                r.Application.VehicleId == _vehicle.VehicleId &&
                r.IsActive == 0 &&
                (application == null || r.ApplicationId != application.ApplicationId));

            if (hasCertificate)
            {
                MessageHelper.MessageCerrentSertificate();
                return false;
            }

            // Проверка на то, что у авто нет другого владельца
            bool hasOtherOwner = App.DbContext.Certificates.Any(r =>
                r.Application.VehicleId == _vehicle.VehicleId &&
                r.IsActive == 0 &&
                (application == null || r.ApplicationId != application.ApplicationId));

            if (hasOtherOwner)
            {
                MessageHelper.MessageCerrentOwner();
                return false;
            }

            // Если ошибок нет, то возвращаем true
            return true;
        }
    }
}
