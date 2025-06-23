using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAccountingGibdd.Classes.Services
{
    public class VehicleService
    {
        // Поля
        private readonly string _vin;
        private readonly string _brand;
        private readonly string _model;
        private readonly short _year;
        private readonly string _color;
        private readonly string _licensePlate;
        private readonly VehicleType _vehicleType;
        private readonly List<PhotosVehicle> _photos;

        // Контруктор
        public VehicleService(string vin, string brand, string model, string year, 
            string color, string licensePlate, VehicleType? vehicleType, List<PhotosVehicle> photos)
        {
            _vin = vin;
            _brand = brand;
            _model = model;
            _year = TypeHelper.ShortParse(year);
            _color = color;
            _licensePlate = licensePlate;
            _vehicleType = vehicleType;
            _photos = photos;
        }

        // Добавление
        public void Add()
        {
            var vehicle = new Vehicle
            {
                Vin = _vin,
                Brand = _brand,
                Model = _model,
                Year = _year,
                Color = _color,
                LicensePlate = _licensePlate,
                VehicleTypeId = _vehicleType.VehicleTypeId
            };

            App.DbContext.Add(vehicle); 
            App.DbContext.SaveChanges();

            // Добавляем фото
            foreach (var photo in _photos)
            {
                var photoEntity = new PhotosVehicle
                {
                    VehicleId = vehicle.VehicleId,
                    Photo = photo.Photo
                };
                App.DbContext.Add(photoEntity);
            }
            App.DbContext.SaveChanges();
        }

        // Редактированиие
        public void Update(Vehicle vehicle)
        {
            vehicle.Vin = _vin;
            vehicle.Brand = _brand;
            vehicle.Model = _model;
            vehicle.Year = _year;
            vehicle.Color = _color;
            vehicle.LicensePlate = _licensePlate;
            vehicle.VehicleTypeId = _vehicleType.VehicleTypeId;

            // Удаляем старые фото
            var oldPhotos = App.DbContext.PhotosVehicles.Where(p => p.VehicleId == vehicle.VehicleId).ToList();
            App.DbContext.PhotosVehicles.RemoveRange(oldPhotos);

            // Добавляем новые фото
            foreach (var photo in _photos)
            {
                var photoEntity = new PhotosVehicle
                {
                    VehicleId = vehicle.VehicleId,
                    Photo = photo.Photo
                };
                App.DbContext.Add(photoEntity);
            }

            App.DbContext.Update(vehicle); 
            App.DbContext.SaveChanges();
        }

        // Удаление 
        public static void Delete(Vehicle vehicle)
        {
            vehicle.Deleted = 1;

            App.DbContext.Update(vehicle);
            App.DbContext.SaveChanges();
        }

        // Отвязка авто от владельца
        public static void DetachOwner(Vehicle vehicle)
        {
            // Находим сертификат
            var certificate = vehicle.Applications
                .SelectMany(a => a.Certificates)
                .Single(c => c.IsActive == 1);

            // Помечаем, как неактивный
            certificate.IsActive = 0;

            App.DbContext.Update(certificate);
            App.DbContext.SaveChanges();
        }

        // Проверка для создания
        public bool Check()
        {
            // Пустые поля
            bool hasNullFields = new[] { _vin, _brand, _model, _color }.Any(string.IsNullOrWhiteSpace);
            if (hasNullFields || _year == -1 || _vehicleType == null)
            {
                MessageHelper.MessageNullFields();
                return false;
            }

            // Длина строк
            bool isShortNames = _brand.Length < 3 || _model.Length < 3 || _color.Length < 3;
            if (isShortNames)
            {
                MessageHelper.MessageUniversal("Наименование марки, модели и цвета должны быть более 3 символов!");
                return false;
            }

            // Валидность ВИН
            bool isVinValid = Validations.ValidateCorrectVin(_vin);
            if (!isVinValid)
            {
                MessageHelper.MessageInvalidVin();
                return false;
            }

            // Если есть номерной знак
            if (!string.IsNullOrEmpty(_licensePlate))
            {
            // Валидность номерного знака
                bool isValidLicensePlate = Validations.ValidateCorrectLicensePlate(_licensePlate);
                if (!isValidLicensePlate)
                {
                    MessageHelper.MessageInvalidLicensePlate();
                    return false;
                }

                // Повторяющийся номерной знак
                bool isDuplicateLicensePlate = App.DbContext.Vehicles.Any(v => v.LicensePlate == _licensePlate);
                if (isDuplicateLicensePlate)
                {
                    MessageHelper.MessageDuplicateLicensePlate();
                    return false;
                }
            }

            // Не релевантый год
            bool isRelevantYear = 1950 < _year && _year < DateTime.Now.Year;
            if (!isRelevantYear)
            {
                MessageHelper.MessageUniversal("Год должен быть в промежутке от 1950 года до настоящего!");
                return false;
            }

            // Повторяющийся VIN
            bool isDuplicateVin = App.DbContext.Vehicles.Any(v => v.Vin == _vin);
            if (isDuplicateVin)
            {
                MessageHelper.MessageDuplicateVin();
                return false;
            }

            // Если ошибок нет, то возвращаем true
            return true;
        }

        // Проверка для редактирования
        public bool Check(Vehicle vehicle)
        {
            // Пустые поля
            bool hasNullFields = new[] { _vin, _brand, _model, _color }.Any(string.IsNullOrWhiteSpace);
            if (hasNullFields || _year == -1 || _vehicleType == null)
            {
                MessageHelper.MessageNullFields();
                return false;
            }

            // Длина строк
            bool isShortNames = _brand.Length < 3 || _model.Length < 3 || _color.Length < 3;
            if (isShortNames)
            {
                MessageHelper.MessageUniversal("Наименование марки, модели и цвета должны быть более 3 символов!");
                return false;
            }

            // Валидность номерного знака
            bool isLicensePlateValid = ValidateLicensePlateChange(vehicle, _licensePlate);
            if (!isLicensePlateValid)
            {
                return false;
            }

            // Не релевантый год
            bool isRelevantYear = 1950 < _year && _year < DateTime.Now.Year;
            if (!isRelevantYear)
            {
                MessageHelper.MessageUniversal("Год должен быть в промежутке от 1950 года до настоящего!");
                return false;
            }

            // Повторяющийся VIN
            bool isDuplicateVin = App.DbContext.Vehicles.Any(v => v.Vin == _vin && v.VehicleId != vehicle.VehicleId);
            if (isDuplicateVin)
            {
                MessageHelper.MessageDuplicateVin();
                return false;
            }

            // Изменения 
            bool hasNotChanges =
                vehicle.Vin == _vin &&
                vehicle.LicensePlate == _licensePlate &&
                vehicle.Brand == _brand &&
                vehicle.Model == _model &&
                vehicle.Year == _year &&
                vehicle.Color == _color &&
                vehicle.VehicleTypeId == _vehicleType.VehicleTypeId;

            bool areSimilarPhotos = ArePhotoListsEqual(_photos, vehicle.PhotosVehicles.ToList());

            if (hasNotChanges && areSimilarPhotos)
            {
                MessageHelper.MessageNotChanges();
                return false;
            }

            // Если ошибок нет, то возвращаем true
            return true;
        }

        // Проверка на номерные знаки
        private bool ValidateLicensePlateChange(Vehicle vehicle, string newLicensePlate)
        {
            // Валидность на формат
            if (!Validations.ValidateCorrectLicensePlate(newLicensePlate))
            {
                MessageHelper.MessageInvalidLicensePlate();
                return false;
            }

            // Повторяющийся номерной знак
            bool isDuplicateLicensePlate = App.DbContext.Vehicles.Any(v => v.LicensePlate == _licensePlate && v.VehicleId != vehicle.VehicleId);
            if (isDuplicateLicensePlate)
            {
                MessageHelper.MessageDuplicateLicensePlate();
                return false;
            }

            return true; // Все проверки пройдены
        }

        // Проверка на изменение изображений
        private bool ArePhotoListsEqual(List<PhotosVehicle> list1, List<PhotosVehicle> list2)
        {
            // Если оба списка null или пустые — считаем равными
            if ((list1 == null || list1.Count == 0) && (list2 == null || list2.Count == 0))
                return true;

            // Если только один из списков пустой — не равны
            if ((list1 == null || list2 == null) || (list1.Count != list2.Count))
                return false;

            // Сравниваем поэлементно массивы байт
            for (int i = 0; i < list1.Count; i++)
            {
                var img1 = list1[i].Photo;
                var img2 = list2[i].Photo;
                if (!img1.SequenceEqual(img2))
                    return false;
            }
            return true;
        }
    }
}
