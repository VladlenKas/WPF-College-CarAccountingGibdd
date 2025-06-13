using CarAccountingGibdd.Classes.Services;
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
    /// Логика взаимодействия для CertificatePageOperator.xaml
    /// </summary>
    public partial class CertificatePageOperator : Page
    {
        private Employee _operator;
        private CertificatesDataService _dataService;

        public CertificatePageOperator(Employee @operator)
        {
            InitializeComponent();

            _operator = @operator;
            _dataService = new(filterCB, sorterCB, searchTB, ascendingCHB, searchBTN, resetFiltersBTN, UpdateIC);
            UpdateIC();
        }

        // Методы
        private void UpdateIC()
        {
            var certificates = App.DbContext.Certificates.ToList();

            // Фильтры
            certificates = _dataService.ApplyFilter(certificates);
            certificates = _dataService.ApplySort(certificates);
            certificates = _dataService.ApplySearch(certificates);

            cardsIC.Items.Clear();
            foreach (var certificate in certificates)
            {
                var card = new CertificateCard(certificate);
                cardsIC.Items.Add(card);
            }
        }
    }
}
