﻿using CarAccountingGibdd.Classes;
using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Dialogs;
using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarAccountingGibdd.Pages.PagesOperator
{
    /// <summary>
    /// Логика взаимодействия для VehiclePageOperator.xaml
    /// </summary>
    public partial class VehiclePageOperator : Page
    {
        private VehiclesDataService _dataService;
        public VehiclePageOperator()
        {
            InitializeComponent();

            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var vehicles = App.DbContext.Vehicles.Where(r => r.Deleted != 1).ToList();

            // Фильтры
            vehicles = _dataService.ApplyFilter(vehicles);
            vehicles = _dataService.ApplySort(vehicles);
            vehicles = _dataService.ApplySearch(vehicles);

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = vehicles;

            itemsDG.ItemsSource = null;
            itemsDG.ItemsSource = vehicles;
        }

        // Обработчики событий
        private void DetachOwner_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = itemsDG.SelectedItem as Vehicle;

            if (vehicle.Owner == null)
            {
                MessageHelper.MessageUniversal("У данного ТС нет владельца!");
                return;
            }
            
            if (vehicle.Owner != null)
            {
                bool accept = MessageHelper.ConfirmDetachOwner();
                if (!accept) return;

                VehicleService.DetachOwner(vehicle);
                UpdateIC();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = itemsDG.SelectedItem as Vehicle;

            EditVehicleDialog dialog = new(vehicle);
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddVehicleDialog dialog = new();
            ComponentsHelper.ShowDialogWindowDark(dialog);

            bool saved = dialog.Saved;
            if (saved) UpdateIC();
        }

        private void ImageView_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = itemsDG.SelectedItem as Vehicle;
            List<PhotosVehicle> photos = vehicle.PhotosVehicles.ToList();

            ViewImageForVehicleDialog dialog = new(photos);
            ComponentsHelper.ShowDialogWindowDark(dialog);
        }
    }
}
