using CarAccountingGibdd.Model;
using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes.Services
{
    public class OwnerService
    {
        // Поля
        private readonly string _firstname;
        private readonly string _lastname;
        private readonly string _patronymic;
        private readonly DateOnly _birthdate;
        private readonly string _phone;
        private readonly string _passport;
        private readonly string _address;

        // Конструктор 
        public OwnerService(string firstname, string lastname, string patronymic, 
            string birthdate, string phone, string passport, string address)
        {
            _firstname = firstname;
            _lastname = lastname;
            _patronymic = patronymic;
            _birthdate = TypeHelper.DateOnlyParse(birthdate);
            _phone = phone;
            _passport = passport;
            _address = address;
        }

        // Добавление
        public void Add()
        {
            var owner = new Owner
            {
                Firstname = _firstname,
                Lastname = _lastname,
                Patronymic = _patronymic,
                Birthdate = _birthdate,
                Phone = _phone,
                Passport = _passport,
                Address = _address,
                Deleted = 0
            };

            App.DbContext.Add(owner);
            App.DbContext.SaveChanges();
        }

        // Редактирование
        public void Update(Owner owner)
        {
            owner.Firstname = _firstname;
            owner.Lastname = _lastname;
            owner.Patronymic = _patronymic;
            owner.Birthdate = _birthdate;
            owner.Phone = _phone;
            owner.Passport = _passport;
            owner.Address = _address;

            App.DbContext.Update(owner);
            App.DbContext.SaveChanges();
        }

        // Удаление
        public static void Delete(Owner owner)
        {
            owner.Deleted = 1;

            App.DbContext.Update(owner);
            App.DbContext.SaveChanges();
        }

        // Проверка
        public bool Check(Owner? owner = null)
        {
            // Пустые поля  
            bool hasNullFields = new[] { _firstname, _lastname, _phone, _passport, _address }.Any(string.IsNullOrWhiteSpace);
            if (hasNullFields)
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

            // Длина телефона
            bool isShortPhone = _phone.Length != 11;
            if (isShortPhone)
            {
                MessageHelper.MessageShortPhone();
                return false;
            }

            // Длина паспорта
            bool isShortPassport = _passport.Length != 10;
            if (isShortPassport)
            {
                MessageHelper.MessageShortPassport();
                return false;
            }

            // Длина адреса
            bool isShortAddress = _address.Length < 10;
            if (isShortAddress)
            {
                MessageHelper.MessageShortAddress();
                return false;
            }

            // Если добавляем владельца
            if (owner == null)
            {
                // Дубликат телефона
                bool isDublicatePhone = App.DbContext.Owners.Any(o => o.Phone == _phone);
                if (isDublicatePhone)
                {
                    MessageHelper.MessageDuplicatePhone();
                    return false;
                }

                // Дубликат паспорта
                bool isDublicatePassport = App.DbContext.Owners.Any(o => o.Passport == _passport);
                if (isDublicatePassport)
                {
                    MessageHelper.MessageDuplicatePassport();
                    return false;
                } 
            }

            // Если редактируем владельца
            if (owner != null)
            {
                // Дубликат телефона
                bool isDublicatePhone = App.DbContext.Owners.Any(o => o.Phone == _phone && o.OwnerId != owner.OwnerId);
                if (isDublicatePhone)
                {
                    MessageHelper.MessageDuplicatePhone();
                    return false;
                }

                // Дубликат телефона
                bool isDublicatePassport = App.DbContext.Owners.Any(o => o.Passport == _passport && o.OwnerId != owner.OwnerId);
                if (isDublicatePassport)
                {
                    MessageHelper.MessageDuplicatePassport();
                    return false;
                }

                // Изменения 
                bool hasNotChanges =
                    owner.Firstname == _firstname &&
                    owner.Lastname == _lastname &&
                    owner.Patronymic == _patronymic &&
                    owner.Birthdate == _birthdate &&
                    owner.Phone == _phone &&
                    owner.Passport == _passport &&
                    owner.Address == _address;
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
