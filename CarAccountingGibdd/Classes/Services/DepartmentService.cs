using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes.Services
{
    public class DepartmentService
    {
        // Поля
        private readonly string _name;
        private readonly string _phone;
        private readonly string _address;

        // Конструктор
        public DepartmentService(string name, string phone, string address)
        {
            _name = name;
            _phone = phone;
            _address = address;
        }

        // Добавление
        public void Add()
        {
            Department department = new Department()
            {
                Name = _name,
                Phone = _phone,
                Address = _address
            };

            App.DbContext.Add(department);
            App.DbContext.SaveChanges();
        }

        // Редактирование 
        public void Update(Department department)
        {
            department.Name = _name;
            department.Phone = _phone;
            department.Address = _address;

            App.DbContext.Update(department);
            App.DbContext.SaveChanges();
        }

        // Проверка 
        public bool Check(Department? department = null)
        {
            // Пустые поля  
            bool hasNullFields = new[] { _name, _phone, _address }.Any(string.IsNullOrWhiteSpace);
            if (hasNullFields)
            {
                MessageHelper.MessageNullFields();
                return false;
            }

            // Короткое название
            bool isShortName = _name.Length < 3;
            if (isShortName)
            {
                MessageHelper.MessageUniversal("Длина названия должна быть больше 3-х символов!");
                return false;
            }

            // Длина телефона
            bool isShortPhone = _phone.Length != 11;
            if (isShortPhone)
            {
                MessageHelper.MessageShortPhone();
                return false;
            }

            // Короткий адрес
            bool isShortAddress = _address.Length < 10;
            if (isShortAddress)
            {
                MessageHelper.MessageShortAddress();
                return false;
            }

            if (department == null)
            {
                // Дубликат названия
                bool isDublicateName = App.DbContext.Departments.Any(o => o.Name == _name);
                if (isDublicateName)
                {   
                    MessageHelper.MessageUniversal("Такое название департамента уже существует! Введите другое");
                    return false;
                }

                // Дубликат телефона
                bool isDublicatePhone = App.DbContext.Departments.Any(o => o.Phone == _phone);
                if (isDublicatePhone)
                {
                    MessageHelper.MessageDuplicatePhone();
                    return false;
                }

                // Дубликат телефона
                bool isDublicateAddress = App.DbContext.Departments.Any(o => o.Address == _address);
                if (isDublicateAddress)
                {
                    MessageHelper.MessageUniversal("Такой адрес уже занят другим департаментом! Введите другое");
                    return false;
                }
            }

            if (department != null)
            {
                // Дубликат названия
                bool isDublicateName = App.DbContext.Departments.Any(o => o.Name == _name && o.DepartmentId != department.DepartmentId);
                if (isDublicateName)
                {
                    MessageHelper.MessageUniversal("Такое название департамента уже существует! Введите другое");
                    return false;
                }

                // Дубликат телефона
                bool isDublicatePhone = App.DbContext.Departments.Any(o => o.Phone == _phone && o.DepartmentId != department.DepartmentId);
                if (isDublicatePhone)
                {
                    MessageHelper.MessageDuplicatePhone();
                    return false;
                }

                // Дубликат телефона
                bool isDublicateAddress = App.DbContext.Departments.Any(o => o.Address == _address && o.DepartmentId != department.DepartmentId);
                if (isDublicateAddress)
                {
                    MessageHelper.MessageUniversal("Такой адрес уже занят другим департаментом! Введите другое");
                    return false;
                }

                bool hasNotChanges =
                    department.Name == _name &&
                    department.Phone == _phone &&
                    department.Address == _address;
                if (hasNotChanges)
                {
                    MessageHelper.MessageNotChanges();
                    return false;
                }
            }

            // Если ошибок нет, то возвращаем true
            return true;
        }
    }
}
