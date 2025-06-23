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
        // Поля
        private Inspection _inspection;

        // Конструктор 
        public InspectionService(Inspection inspection)
        {
            _inspection = inspection;
        }

        // Начало осмотра
        public void StartInspection()
        {
            _inspection.StatusId = 2;
            _inspection.Application.ApplicationStatusId = 4;

            App.DbContext.Update(_inspection);
            App.DbContext.SaveChanges();
        }

        // Отклонение
        public void Reject()
        {
            _inspection.StatusId = 5;
            _inspection.Application.ApplicationStatusId = 5;
            _inspection.DatetimeCompleted = DateTime.Now;

            ViolationInspection violationsInspection = new ViolationInspection()
            {
                InspectionId = _inspection.InspectionId,
                ViolationId = 1
            };

            App.DbContext.Add(violationsInspection);
            App.DbContext.Update(_inspection);
            App.DbContext.SaveChanges();
        }

        // Формирование свидетельства
        public Certificate CreateCertificate(string number, string newLicensePlate)
        {
            // Формируем свидетельство
            Certificate certificate = new Certificate
            {
                ApplicationId = _inspection.ApplicationId,
                IssueDate = DateOnly.FromDateTime(DateTime.Now),
                LicensePlate = newLicensePlate,
                Number = number,
                IsActive = 1
            };

            // Меняем данные ТС
            ChangeLicensePlate(newLicensePlate);

            // Меняем статус инспекции
            _inspection.StatusId = 3; // пройдена
            _inspection.DatetimeCompleted = DateTime.Now;

            // Меняем статус заявки
            _inspection.Application.ApplicationStatusId = 7;

            Vehicle vehicle = _inspection.Application.Vehicle;
            if (vehicle.Used == 0)
            {
                // Получаем всех владельцев из сертификатов текущего ТС
                var owners = _inspection.Application.Certificates
                    .Select(c => c.Application.Owner)
                    .Distinct()
                    .ToList();

                // Проверяем, одинаковые ли владельцы
                bool allOwnersAreSame = owners.Count < 1;

                if (allOwnersAreSame)
                {
                    vehicle.Used = 1;
                }
            }

            App.DbContext.Update(vehicle);
            App.DbContext.Update(_inspection);
            App.DbContext.Add(certificate);
            App.DbContext.SaveChanges();

            return certificate;
        }

        public void CreateViolationsInspection(List<Violation> violations)
        {
            // Все нарушения
            var violationsToAdd = new List<ViolationInspection>();

            // Перебираем их, чтобы указать Id инспекции
            violations.ForEach(v =>
            {
                // Создаём нарушение
                violationsToAdd.Add(new ViolationInspection
                {
                    InspectionId = _inspection.InspectionId,
                    ViolationId = v.ViolationId
                });
            });

            // Меняем статус инспекции
            _inspection.StatusId = 4; // не пройдена
            _inspection.DatetimeCompleted = DateTime.Now;

            // Меняем статус заявки
            _inspection.Application.ApplicationStatusId = 5;

            App.DbContext.Update(_inspection);
            App.DbContext.AddRange(violationsToAdd);
            App.DbContext.SaveChanges();
        }


        // Смена знака машины на новый
        private void ChangeLicensePlate(string newLicensePlate)
        {
            var vehicle = _inspection.Application.Vehicle;
            vehicle.LicensePlate = newLicensePlate;

            App.DbContext.Update(vehicle);
            App.DbContext.SaveChanges();
        }
    }
}
