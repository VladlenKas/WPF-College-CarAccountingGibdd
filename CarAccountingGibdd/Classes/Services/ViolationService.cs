using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes.Services
{
    public class ViolationService
    {
        // Поля
        private readonly string _description;

        // Конструктор
        public ViolationService(string description)
        {
            _description = description;
        }

        // Добавление
        public void Add()
        {
            int number = App.DbContext.AllViolations
                .OrderBy(v => v.ViolationId)
                .Last()
                .Number;

            Violation violation = new Violation()
            {
                Number = number + 1,
                Description = _description
            };

            App.DbContext.Add(violation);
            App.DbContext.SaveChanges();
        }

        // Редактирование
        public void Update(Violation violation)
        {
            violation.Description = _description;

            App.DbContext.Update(violation);
            App.DbContext.SaveChanges();
        }

        // Редактирование
        public bool Check(Violation? violation = null)
        {
            // Пустые поля
            if (string.IsNullOrWhiteSpace(_description))
            {
                MessageHelper.MessageNullFields();
                return false;
            }

            // Если добавление
            if (violation == null)
            {
                bool isViolationDublicate = App.DbContext.Violations.Any(v => v.Description == _description);
                if (isViolationDublicate)
                {
                    MessageHelper.MessageUniversal("Нарушение с таким описанием уже существует!");
                    return false;
                }
            }

            // Если редактирование
            if (violation != null)
            {
                bool isViolationDublicate = App.DbContext.Violations.Any(v => v.Description == _description && v.ViolationId != violation.ViolationId);
                if (isViolationDublicate)
                {
                    MessageHelper.MessageUniversal("Нарушение с таким описанием уже существует!");
                    return false;
                }

                // Изменения
                bool hasNotChanges =
                    violation.Description == _description;
                if (hasNotChanges)
                {
                    MessageHelper.MessageNotChanges();
                    return false;
                }
            }

            // Если ошибок нет
            return true;
        }
    }
}
