﻿using CarAccountingGibdd.Classes.Services;
using CarAccountingGibdd.Controls;
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
    /// Логика взаимодействия для ViolationsInspectionsPageOperator.xaml
    /// </summary>
    public partial class ViolationsInspectionsPageOperator : Page
    {
        private ViolationsInspectionsDataService _dataService;
        // Поля
        private Employee _operator;

        // Конструктор

        public ViolationsInspectionsPageOperator(Employee @operator)
        {
            InitializeComponent();

            _operator = @operator;
            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var violationsInspections = App.DbContext.ViolationInspection
                .GroupBy(r => r.InspectionId)
                .ToList();

            // Фильтры
            violationsInspections = _dataService.ApplyAccessControl(violationsInspections, _operator);
            violationsInspections = _dataService.ApplyFilter(violationsInspections);
            violationsInspections = _dataService.ApplySort(violationsInspections);
            violationsInspections = _dataService.ApplySearch(violationsInspections);

            cardsIC.Items.Clear();
            foreach (var violationsInspection in violationsInspections)
            {
                cardsIC.Items.Add(new ViolationsInspectionCard(violationsInspection, _operator));
            }
        }
    }
}
