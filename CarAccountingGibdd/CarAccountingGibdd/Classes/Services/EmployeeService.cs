using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes.Services
{
    public class EmployeeService
    {
        // Поля
        private readonly string _firstname;
        private readonly string _lastname;
        private readonly string _patronymic;
        private readonly DateOnly _birthdate;
        private readonly string _passport;
        private readonly Department? _department;
        private readonly Post? _post;
        private readonly string _email;
        private readonly string _password;

        // Конструктор
        public EmployeeService(string firstname, string lastname, string patronymic, DateOnly birthdate, 
            string passport, Department? department, Post? post, string email, string password)
        {
            _firstname = firstname;
            _lastname = lastname;
            _patronymic = patronymic;
            _birthdate = birthdate;
            _passport = passport;
            _department = department;
            _post = post;
            _email = email;
            _password = password;
        }

        // Добавление
        public void Add()
        {
            Employee employee = new Employee()
            {
                Firstname = _firstname,
                Lastname = _lastname,
                Patronymic = _patronymic,
                Birthdate = _birthdate,
                Passport = _passport,
                DepartmentId = _department.DepartmentId,
                PostId = _post.PostId,
                Email = _email,
                Password = _password
            };

            App.DbContext.Add(employee);
            App.DbContext.SaveChanges();
        }

        // Редактирование
        public void Update(Employee employee)
        {
            employee.Firstname = _firstname;
            employee.Lastname = _lastname;
            employee.Patronymic = _patronymic;
            employee.Birthdate = _birthdate;
            employee.Passport = _passport;
            employee.DepartmentId = _department.DepartmentId;
            employee.PostId = _post.PostId;
            employee.Email = _email;
            employee.Password = _password;

            App.DbContext.Update(employee);
            App.DbContext.SaveChanges();
        }

        // Проверка
        public async Task<bool> CheckAsync(Employee? employee = null)
        {
            // Пустые поля  
            bool hasNullFields = new[] { _firstname, _lastname, _passport, _email, _password }.Any(string.IsNullOrWhiteSpace);
            if (hasNullFields || _post == null || _department == null)
            {
                MessageHelper.MessageNullFields();
                return false;
            }


            // Длина ФИО
            bool isShortFullname = _firstname.Length < 3 || _firstname.Length < 3 || (_patronymic != string.Empty && _patronymic.Length < 3);
            if (isShortFullname)
            {
                MessageHelper.MessageShortFio();
                return false;
            }

            // Наличие даты рождения
            bool hasNullBirthdate = _birthdate == DateOnly.MinValue;
            if (hasNullBirthdate)
            {
                MessageHelper.MessageNullDate();
                return false;
            }

            // Допустимый возраст
            bool isInappropriateAge = !Validations.ValidateCorrectAge(_birthdate);
            if (isInappropriateAge)
            {
                MessageHelper.MessageInappropriateAge();
                return false;
            }

            // Валидность почты
            if (!string.IsNullOrWhiteSpace(_email))
            {
                bool isValidEmail = await Validations.ValidateEmailAsync(_email);
                if (!isValidEmail)
                {
                    MessageHelper.MessageInvalidEmail();
                    return false;
                }
            }

            // Длина паспорта
            bool isShortPassport = _passport.Length != 10;
            if (isShortPassport)
            {
                MessageHelper.MessageShortPassport();
                return false;
            }

            // Длина пароля
            bool isShortPasword = _password.Length < 3;
            if (isShortPasword)
            {
                MessageHelper.MessageUniversal("Длина пароля должна быть не менее 3-х символов!");
                return false;
            }

            // Если добавляем владельца
            if (employee == null)
            {
                // Дубликат почты
                bool isDublicateEmail = App.DbContext.Employees.Any(o => o.Email == _email);
                if (isDublicateEmail)
                {
                    MessageHelper.MessageDuplicateEmail();
                    return false;
                }

                // Дубликат паспорта
                bool isDublicatePassport = App.DbContext.Employees.Any(o => o.Passport == _passport);
                if (isDublicatePassport)
                {
                    MessageHelper.MessageDuplicatePassport();
                    return false;
                }
            }

            // Если редактируем владельца
            if (employee != null)
            {
                // Дубликат почты
                bool isDublicateEmail = App.DbContext.Employees.Any(o => o.Email == _email && o.EmployeeId != employee.EmployeeId);
                if (isDublicateEmail)
                {
                    MessageHelper.MessageDuplicateEmail();
                    return false;
                }

                // Дубликат паспорта
                bool isDublicatePassport = App.DbContext.Employees.Any(o => o.Passport == _passport && o.EmployeeId != employee.EmployeeId);
                if (isDublicatePassport)
                {
                    MessageHelper.MessageDuplicatePassport();
                    return false;
                }

                // Изменения 
                bool hasNotChanges =
                    employee.Firstname == _firstname &&
                    employee.Lastname == _lastname &&
                    Validations.StringEqualsNullOrEmpty(employee.Patronymic, _patronymic) &&
                    employee.Birthdate == _birthdate &&
                    Validations.StringEqualsNullOrEmpty(employee.Email, _email) &&
                    employee.Passport == _passport &&
                    employee.Password == _password &&
                    employee.DepartmentId == _department.DepartmentId &&
                    employee.PostId == _post.PostId;
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
