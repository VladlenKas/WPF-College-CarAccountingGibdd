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
        // Поля
        private readonly Owner _owner;
        private readonly Vehicle _vehicle;
        private readonly int _paymentMethod;
        private readonly int _change;
        private readonly Card? _card;

        // Конструктор для добавления
        public ApplicationService(Owner owner, Vehicle vehicle, int paymentMethod, int change, Card? card) 
        {
            _owner = owner;
            _vehicle = vehicle;
            _paymentMethod = paymentMethod;
            _change = change;
            _card = card;
        }

        // Конструктор для редактирования
        public ApplicationService(Owner owner, Vehicle vehicle) 
        {
            _owner = owner;
            _vehicle = vehicle;
        }

        // Добавление
        public void Create(Employee @operator)
        {
            Application application = new()
            {
                OwnerId = _owner.OwnerId,
                VehicleId = _vehicle.VehicleId,
                ApplicationStatusId = 1, // На проверке документов
                Amount = 400,
                PaymentMethod = (sbyte)_paymentMethod,
                DatetimeSupply = DateTime.Now
            };

            App.DbContext.Add(application);
            App.DbContext.SaveChanges();
        }

        // Редактирование
        public void Edit(Application application)
        {
            application.OwnerId = _owner.OwnerId;
            application.VehicleId = _vehicle.VehicleId;

            App.DbContext.Update(application);
            App.DbContext.SaveChanges();
        }

        // Подтверждение
        public static void Confirm(Application application, Employee @operator)
        {
            application.ApplicationStatusId = 2;
            application.DatetimeConfirm = DateTime.Now;

            App.DbContext.Update(application);
            App.DbContext.SaveChanges();
        }

        // Принятие на инспекцию
        public static void Accept(Application application, Employee inspector, DateTime dateTimePlanned)
        {
            application.ApplicationStatusId = 3;

            App.DbContext.Update(application);
            App.DbContext.SaveChanges();

            Inspection inspection = new()
            {
                ApplicationId = application.ApplicationId,
                InspectorId = inspector.EmployeeId,
                InspectionStatusId = 1,
                DatetimePlanned = dateTimePlanned,
            };

            App.DbContext.Add(inspection);
            App.DbContext.SaveChanges();
        }

        // Отклонение
        public static void Reject(Application application, Employee @operator)
        {
            application.ApplicationStatusId = 6;
            application.DatetimeConfirm = DateTime.Now;

            App.DbContext.Update(application);
            App.DbContext.SaveChanges();
        }

        // Просрочка 
        private static void Overdue(List<Inspection> overdueInspections)
        {
            // Обновляем статусы
            overdueInspections.ForEach(s =>
            {
                s.InspectionStatusId = 2; // В процессе
                s.Application.ApplicationStatusId = 4; // На осмотре
            });

            // Обновляем и сохраняем всё одним запросом
            App.DbContext.UpdateRange(overdueInspections);
            App.DbContext.SaveChanges();
        }

        // Проверка для редактирования
        public bool Check(Application application)
        {
            // Проверки на пустые поля
            bool nullFields = _owner == null || _vehicle == null;

            if (nullFields)
            {
                MessageHelper.MessageNullFields();
                return false;
            }

            // Проверка на изменения 
            bool noChanges = 
                application.OwnerId == _owner.OwnerId &&
                application.VehicleId == _vehicle.VehicleId;

            if (noChanges)
            {
                MessageHelper.MessageNotChanges();
                return false;
            }

            // Если ошибок нет, то возвращаем true
            return true;
        }

        // Проверка для добавления
        public bool Check()
        {
            // Проверки на пустые поля
            bool nullFields = _owner == null || _vehicle == null || _paymentMethod == -1 || (_paymentMethod == 0 && _card == null);
            bool nullPay = _paymentMethod == 1 && _change < 0;
            if (nullFields)
            {
                MessageHelper.MessageNullFields();
                return false;
            }
            else if (nullPay)
            {
                MessageHelper.MessageNullCost();
                return false;
            }

            // Проверка на ввод данных карты
            if (_card != null)
            {
                bool isNumberCardCorrect = Validations.ValidateCardNumber(_card);
                if (!isNumberCardCorrect)
                {
                    return false;
                }
            }

            // Если ошибок нет, то возвращаем true
            return true;
        }

        // Проверка просрочки 
        public static void HasStartedInspections()
        {
            List<Inspection> startedInspections = App.DbContext.Inspections
                .Where(d => 
                    d.DatetimePlanned.AddHours(2) < DateTime.Now &&
                    d.InspectionStatusId == 1)
                .ToList();

            bool hasStartedInspections = startedInspections.Count > 0;
            if (hasStartedInspections)
            {
                Overdue(startedInspections);
            }
        }
    }
}
