using CarAccountingGibdd.Components;
using CarAccountingGibdd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static iTextSharp.text.pdf.hyphenation.TernaryTree;
using static System.Net.Mime.MediaTypeNames;

namespace CarAccountingGibdd.Classes.Services;

public class ApplicationDataService
{
    private ComboBox _filterCB;
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public ApplicationDataService(ComboBox filterCB, ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _filterCB = filterCB;
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        filterCB.ItemsSource = new[] { "Все заявки", "Активные заявки", "Завершенные заявки" };
        filterCB.SelectedIndex = 1;
        sorterCB.ItemsSource = new[] { "По дате подачи", "По номеру заявки", "По ФИО владельца", "По инфо. ТС", "По статусу" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = false;

        OnTriggers();
    }

    // Поиск
    public List<Model.Application> ApplySearch(List<Model.Application> applications)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            applications = applications.Where(r =>
                r.ApplicationId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.DatetimeSupply.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.DatetimeAccept?.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.DatetimeConfirm?.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.ApplicationStatus.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.DepartmentId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Department.Name.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Department.Address.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Owner.Fullname.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Owner.Passport.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Owner.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Owner.Email?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Vehicle.FullInfo.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();
        }
        return applications;
    }

    // Сортировка
    public List<Model.Application> ApplySort(List<Model.Application> applications)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => applications.OrderBy(e => e.ApplicationId).ToList(),
                2 => applications.OrderBy(e => e.Owner.Fullname).ToList(),
                3 => applications.OrderBy(e => e.Vehicle.BrandModel).ToList(),
                4 => applications.OrderBy(e => e.ApplicationStatusId).ToList(),
                _ => applications.OrderBy(e => e.DatetimeSupply).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => applications.OrderByDescending(e => e.ApplicationId).ToList(),
                2 => applications.OrderByDescending(e => e.Owner.Fullname).ToList(),
                3 => applications.OrderByDescending(e => e.Vehicle.BrandModel).ToList(),
                4 => applications.OrderByDescending(e => e.ApplicationStatusId).ToList(),
                _ => applications.OrderByDescending(e => e.DatetimeSupply).ToList(),
            };
        }
    }

    // Фильтрация
    public List<Model.Application> ApplyFilter(List<Model.Application> applications)
    {
        int dateIndex = _filterCB.SelectedIndex;

        switch (dateIndex)
        {
            case 1: // "Активные заявки"
                return applications
                    .Where(r => 
                        r.ApplicationStatusId == 1 || 
                        r.ApplicationStatusId == 2 ||
                        r.ApplicationStatusId == 3 ||
                        r.ApplicationStatusId == 4)
                    .ToList();

            case 2: // "Завершенные заявки"
                return applications
                    .Where(r => 
                        r.ApplicationStatusId == 5 ||
                        r.ApplicationStatusId == 6 ||
                        r.ApplicationStatusId == 7)
                    .ToList();

            default: // "Все заявки"
                return applications.ToList();
        }
    }

    // Разделение функционала
    public List<Model.Application> ApplyAccessControl(List<Model.Application> applications, Employee employee)
    {
        /*
            2. Инспектор => В своем департаменте ЛИБО свои ЛИБО не принятые
            3. Оператор => Все в своем департаменте
            */

        switch (employee.PostId)
        {
            case 2:
                return applications
                    .Where(app =>
                        (app.DepartmentId == employee.DepartmentId && app.ApplicationStatusId == 2) ||
                        app.Inspections.Any(ins => ins.InspectorId == employee.EmployeeId))
                    .ToList();

            case 3:
                return applications
                    .Where(r => r.DepartmentId == employee.DepartmentId)
                    .ToList();

            default:
                return applications.ToList();
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _filterCB.SelectedIndex = 1;
        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = false;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }


    // Включает тригеры
    private void OnTriggers()
    {
        _filterCB.SelectionChanged += FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _filterCB.SelectionChanged -= FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class InspectionDataService
{
    private ComboBox _filterCB;
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public InspectionDataService(ComboBox filterCB, ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _filterCB = filterCB;
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        filterCB.ItemsSource = new[] { "Все осмотры", "Активные осмотры", "Завершенные осмотры" };
        filterCB.SelectedIndex = 1;
        sorterCB.ItemsSource = new[] { "По дате", "По номеру осмотра", "По номеру заявки", "По ФИО владельца", "По инфо. ТС", "По статусу" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = false;

        OnTriggers();
    }

    // Поиск
    public List<Model.Inspection> ApplySearch(List<Inspection> inspections)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            inspections = inspections.Where(r =>
                r.InspectionId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.DatetimePlanned.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.DatetimeCompleted?.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Inspector.Fullname.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Status.Name.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Application.DepartmentId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Application.Department.Name.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Application.Department.Address.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Application.Owner.Fullname.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Application.Owner.Passport.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Application.Owner.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Application.Owner.Email?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Application.Vehicle.FullInfo.Contains(search, StringComparison.OrdinalIgnoreCase) 
                ).ToList();
        }
        return inspections;
    }

    // Сортировка
    public List<Inspection> ApplySort(List<Inspection> inspections)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => inspections.OrderBy(e => e.InspectionId).ToList(),
                2 => inspections.OrderBy(e => e.ApplicationId).ToList(),
                3 => inspections.OrderBy(e => e.Application.Owner.Fullname).ToList(),
                4 => inspections.OrderBy(e => e.Application.Vehicle.Info).ToList(),
                5 => inspections.OrderBy(e => e.Status.Name).ToList(),
                _ => inspections.OrderBy(e => e.DatetimePlanned).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => inspections.OrderByDescending(e => e.InspectionId).ToList(),
                2 => inspections.OrderByDescending(e => e.ApplicationId).ToList(),
                3 => inspections.OrderByDescending(e => e.Application.Owner.Fullname).ToList(),
                4 => inspections.OrderByDescending(e => e.Application.Vehicle.Info).ToList(),
                5 => inspections.OrderByDescending(e => e.Status.Name).ToList(),
                _ => inspections.OrderByDescending(e => e.DatetimePlanned).ToList(),
            };
        }
    }

    // Фильтрация
    public List<Inspection> ApplyFilter(List<Inspection> inspections)
    {
        int dateIndex = _filterCB.SelectedIndex;

        switch (dateIndex)
        {
            case 1: // "Активные осмотры"
                return inspections
                    .Where(r =>
                        r.StatusId == 1 ||
                        r.StatusId == 2)
                    .ToList();

            case 2: // "Завершенные осмотры"
                return inspections
                    .Where(r =>
                        r.StatusId == 3 ||
                        r.StatusId == 4 ||
                        r.StatusId == 5)
                    .ToList();

            default: // "Все осмотры"
                return inspections.ToList();
        }
    }

    // Фильтр по незначенным осмотрам
    public List<Model.Inspection> ApplyAccessControl(List<Inspection> inspections, Employee employee)
    {
        /*
            2. Инспектор => Только свои
            3. Оператор => Все в своем департаменте
            */

        switch (employee.PostId)
        {
            case 2:
                return inspections
                    .Where(app => app.InspectorId == employee.EmployeeId)
                    .ToList();

            case 3:
                return inspections
                    .Where(app => app.Application.DepartmentId == employee.DepartmentId)
                    .ToList();

            default:
                return inspections.ToList();
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _filterCB.SelectedIndex = 1;
        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = false;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }


    // Включает тригеры
    private void OnTriggers()
    {
        _filterCB.SelectionChanged += FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _filterCB.SelectionChanged -= FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class EmployeesDataService
{
    private ComboBox _filterCB;
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public EmployeesDataService(ComboBox filterCB, ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _filterCB = filterCB;
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        filterCB.ItemsSource = new[] { "Все сотрудники", "Администраторы", "Инспекторы", "Операторы" };
        filterCB.SelectedIndex = 0;
        sorterCB.ItemsSource = new[] { "По ФИО", "По возрасту", "По департаменту" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = false;

        OnTriggers();
    }

    // Поиск
    public List<Employee> ApplySearch(List<Employee> employees)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            employees = employees.Where(r =>
                r.Fullname.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Birthdate.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Department.Name.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Post.Name.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Passport.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Password.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();
        }
        return employees;
    }

    // Сортировка
    public List<Employee> ApplySort(List<Employee> employees)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => employees.OrderBy(e => e.Birthdate).ToList(),
                2 => employees.OrderBy(e => e.Department.Name).ToList(),
                _ => employees.OrderBy(e => e.Fullname).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => employees.OrderByDescending(e => e.Birthdate).ToList(),
                2 => employees.OrderByDescending(e => e.Department.Name).ToList(),
                _ => employees.OrderByDescending(e => e.Fullname).ToList(),
            };
        }
    }

    // Фильтрация
    public List<Employee> ApplyFilter(List<Employee> employees)
    {
        int dateIndex = _filterCB.SelectedIndex;

        switch (dateIndex)
        {
            case 1: // "Админы"
                return employees
                    .Where(r => r.PostId == 1)
                    .ToList();

            case 2: // "Инспекторы"
                return employees
                    .Where(r => r.PostId == 2)
                    .ToList();

            case 3: // "Операторы"
                return employees
                    .Where(r => r.PostId == 3)
                    .ToList();

            default: // "Все"
                return employees.Where(r => r.Deleted != 1).ToList();
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _filterCB.SelectedIndex = 0;
        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = false;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }


    // Включает тригеры
    private void OnTriggers()
    {
        _filterCB.SelectionChanged += FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _filterCB.SelectionChanged -= FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class OwnersDataService
{
    private ComboBox _filterCB;
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public OwnersDataService(ComboBox filterCB, ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _filterCB = filterCB;
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        filterCB.ItemsSource = new[] { "Все владельцы", "С ТС", "Без ТС" };
        filterCB.SelectedIndex = 0;
        sorterCB.ItemsSource = new[] { "По ФИО", "По возрасту", "По адресу" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = false;

        OnTriggers();
    }

    // Поиск
    public List<Owner> ApplySearch(List<Owner> owners)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            owners = owners.Where(r =>
                r.Fullname.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Birthdate.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Passport.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Email?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Address.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();
        }
        return owners;
    }

    // Сортировка
    public List<Owner> ApplySort(List<Owner> owners)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => owners.OrderBy(e => e.Birthdate).ToList(),
                2 => owners.OrderBy(e => e.Address).ToList(),
                _ => owners.OrderBy(e => e.Fullname).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => owners.OrderByDescending(e => e.Birthdate).ToList(),
                2 => owners.OrderByDescending(e => e.Address).ToList(),
                _ => owners.OrderByDescending(e => e.Fullname).ToList(),
            };
        }
    }

    // Фильтрация
    public List<Owner> ApplyFilter(List<Owner> owners)
    {
        int dateIndex = _filterCB.SelectedIndex;

        switch (dateIndex)
        {
            case 1: // "Есть авто"
                return owners
                    .Where(r => r.VehiclesCount < 0)
                    .ToList();

            case 2: // "Нет авто"
                return owners
                    .Where(r => r.VehiclesCount >= 0)
                    .ToList();

            default: // "Все"
                return owners.Where(r => r.Deleted != 1).ToList();
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _filterCB.SelectedIndex = 0;
        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = false;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }


    // Включает тригеры
    private void OnTriggers()
    {
        _filterCB.SelectionChanged += FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _filterCB.SelectionChanged -= FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class VehiclesDataService
{
    private ComboBox _filterCB;
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public VehiclesDataService(ComboBox filterCB, ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _filterCB = filterCB;
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        filterCB.ItemsSource = new[] { "Все ТС", "С владельцем", "Без владельца" };
        filterCB.SelectedIndex = 0;
        sorterCB.ItemsSource = new[] { "По марке", "По модели", "По году", "По цвету", "По типу" };
        sorterCB.SelectedIndex = 4;
        ascendingCHB.IsChecked = true;

        OnTriggers();
    }

    // Поиск
    public List<Vehicle> ApplySearch(List<Vehicle> vehicles)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            vehicles = vehicles.Where(r =>
                r.Brand.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Model.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Year.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Vin.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.LicensePlate?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.VehicleType.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();
        }
        return vehicles;
    }

    // Сортировка
    public List<Vehicle> ApplySort(List<Vehicle> vehicles)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => vehicles.OrderBy(e => e.Model).ToList(),
                2 => vehicles.OrderBy(e => e.Year).ToList(),
                3 => vehicles.OrderBy(e => e.Color).ToList(),
                4 => vehicles.OrderBy(e => e.VehicleType.Name).ToList(),
                _ => vehicles.OrderBy(e => e.Brand).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => vehicles.OrderByDescending(e => e.Model).ToList(),
                2 => vehicles.OrderByDescending(e => e.Year).ToList(),
                3 => vehicles.OrderByDescending(e => e.Color).ToList(),
                4 => vehicles.OrderByDescending(e => e.VehicleType.Name).ToList(),
                _ => vehicles.OrderByDescending(e => e.Brand).ToList(),
            };
        }
    }

    // Фильтрация
    public List<Vehicle> ApplyFilter(List<Vehicle> vehicles)
    {
        int dateIndex = _filterCB.SelectedIndex;

        switch (dateIndex)
        {
            case 1: // "Есть владелец"
                return vehicles
                    .Where(r => r.Owner != null)
                    .ToList();

            case 2: // "Нет владельца"
                return vehicles
                    .Where(r => r.Owner == null)
                    .ToList();

            default: // "Все"
                return vehicles.Where(r => r.Deleted != 1).ToList();
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _filterCB.SelectedIndex = 0;
        _sorterCB.SelectedIndex = 4;
        _ascendingCHB.IsChecked = true;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }


    // Включает тригеры
    private void OnTriggers()
    {
        _filterCB.SelectionChanged += FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _filterCB.SelectionChanged -= FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class ViolationDataService
{
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public ViolationDataService(ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        sorterCB.ItemsSource = new[] { "По номеру", "По описанию" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = true;

        OnTriggers();
    }

    // Поиск
    public List<Violation> ApplySearch(List<Violation> violation)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            violation = violation.Where(r =>
                r.Number.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Description.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) 
                ).ToList();
        }
        return violation;
    }

    // Сортировка
    public List<Violation> ApplySort(List<Violation> violation)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => violation.OrderBy(e => e.Description).ToList(),
                _ => violation.OrderBy(e => e.ViolationId).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => violation.OrderByDescending(e => e.Description).ToList(),
                _ => violation.OrderByDescending(e => e.ViolationId).ToList(),
            };
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = true;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }

    // Включает тригеры
    private void OnTriggers()
    {
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class CertificatesDataService
{
    private ComboBox _filterCB;
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public CertificatesDataService(ComboBox filterCB, ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _filterCB = filterCB;
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        filterCB.ItemsSource = new[] { "Все свидетельства", "Действительные", "Истекшие" };
        filterCB.SelectedIndex = 1;
        sorterCB.ItemsSource = new[] { "По дате", "По номеру", "По номеру заявки", "По ФИО владельца", "По инфо. ТС" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = false;

        OnTriggers();
    }

    // Поиск
    public List<Certificate> ApplySearch(List<Certificate> certificates)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            certificates = certificates.Where(r =>
                r.CertificateId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Number.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.IssueDate.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Application.Owner.Fullname.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Application.Owner.Passport.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Application.Owner.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Application.Owner.Email?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Application.Vehicle.FullInfo.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Application.InspectionNumber?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.Application.InspectorFullname?.Contains(search, StringComparison.OrdinalIgnoreCase) == true 
                ).ToList();
        }
        return certificates;
    }

    // Сортировка
    public List<Certificate> ApplySort(List<Certificate> certificates)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => certificates.OrderBy(e => e.CertificateId).ToList(),
                2 => certificates.OrderBy(e => e.ApplicationId).ToList(),
                3 => certificates.OrderBy(e => e.Application.Owner.Fullname).ToList(),
                4 => certificates.OrderBy(e => e.Application.Vehicle.BrandModel).ToList(),
                _ => certificates.OrderBy(e => e.IssueDate).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => certificates.OrderByDescending(e => e.CertificateId).ToList(),
                2 => certificates.OrderByDescending(e => e.ApplicationId.ToString()).ToList(),
                3 => certificates.OrderByDescending(e => e.Application.Owner.Fullname).ToList(),
                4 => certificates.OrderByDescending(e => e.Application.Vehicle.BrandModel).ToList(),
                _ => certificates.OrderByDescending(e => e.IssueDate).ToList(),
            };
        }
    }

    // Фильтрация
    public List<Certificate> ApplyFilter(List<Certificate> certificates)
    {
        int dateIndex = _filterCB.SelectedIndex;

        switch (dateIndex)
        {
            case 1: // "Действ"
                return certificates
                    .Where(r => r.IsActive == 1)
                    .ToList();

            case 2: // "Истекшие"
                return certificates
                    .Where(r => r.IsActive == 0)
                    .ToList();

            default: // "Все"
                return certificates.ToList();
        }
    }

    // Разделение функционала
    public List<Certificate> ApplyAccessControl(List<Certificate> certificates, Employee employee)
    {
        /*
            2. Инспектор => Только свои
            3. Оператор => Все в своем департаменте
            */

        switch (employee.PostId)
        {
            case 2:
                return certificates
                    .Where(app => app.Application.InspectorId == employee.EmployeeId)
                    .ToList();

            case 3:
                return certificates
                    .Where(r => r.Application.DepartmentId == employee.DepartmentId)
                    .ToList();

            default:
                return certificates.ToList();
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _filterCB.SelectedIndex = 1;
        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = false;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }


    // Включает тригеры
    private void OnTriggers()
    {
        _filterCB.SelectionChanged += FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _filterCB.SelectionChanged -= FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class ViolationsInspectionsDataService
{
    private ComboBox _filterCB;
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public ViolationsInspectionsDataService(ComboBox filterCB, ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _filterCB = filterCB;
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        filterCB.ItemsSource = new[] { "Все списки", "За последние 7 дней", "За последний месяц", "За последний год" };
        filterCB.SelectedIndex = 1;
        sorterCB.ItemsSource = new[] { "По дате", "По ФИО владельца", "По ФИО инспектора", "По названию ТС", "По кол-ву нарушенй" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = false;

        OnTriggers();
    }

    // Поиск
    public List<IGrouping<int, ViolationInspection>> ApplySearch(List<IGrouping<int, ViolationInspection>> violationsInspections)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            violationsInspections = violationsInspections.Where(r =>
                r.First().Inspection.Application.Owner?.Fullname.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.First().Inspection.Application.Vehicle?.FullInfo.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.First().Inspection.ApplicationId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.First().Violation.Description.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.First().Inspection.Application.InspectionNumber?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                r.First().Inspection.Application.InspectorFullname?.Contains(search, StringComparison.OrdinalIgnoreCase) == true
                ).ToList();
        }
        return violationsInspections;
    }

    // Сортировка
    public List<IGrouping<int, ViolationInspection>> ApplySort(List<IGrouping<int, ViolationInspection>> violationsInspections)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => violationsInspections.OrderBy(e => e.First().Inspection.Application.Owner?.Fullname).ToList(),
                2 => violationsInspections.OrderBy(e => e.First().Inspection.Application.InspectorFullname).ToList(),
                3 => violationsInspections.OrderBy(e => e.First().Inspection.Application.Vehicle.BrandModel).ToList(),
                4 => violationsInspections.OrderBy(e => e.Count()).ToList(),
                _ => violationsInspections.OrderBy(e => e.First().Inspection.DatetimeCompleted).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => violationsInspections.OrderByDescending(e => e.First().Inspection.Application.Owner?.Fullname).ToList(),
                2 => violationsInspections.OrderByDescending(e => e.First().Inspection.Application.InspectorFullname).ToList(),
                3 => violationsInspections.OrderByDescending(e => e.First().Inspection.Application.Vehicle.BrandModel).ToList(),
                4 => violationsInspections.OrderByDescending(e => e.Count()).ToList(),
                _ => violationsInspections.OrderByDescending(e => e.First().Inspection.DatetimeCompleted).ToList(),
            };
        }
    }

    // Фильтрация
    public List<IGrouping<int, ViolationInspection>> ApplyFilter(List<IGrouping<int, ViolationInspection>> violationsInspections)
    {
        int dateIndex = _filterCB.SelectedIndex;
        DateTime today = DateTime.Today;

        switch (dateIndex)
        {
            case 1: // За последние 7 дней
                return violationsInspections
                    .Where(list => list.FirstOrDefault()?.Inspection.DatetimeCompleted >= today.AddDays(-7))
                    .ToList();

            case 2: // За последний месяц
                return violationsInspections
                    .Where(list => list.FirstOrDefault()?.Inspection.DatetimeCompleted >= today.AddMonths(-1))
                    .ToList();

            case 3: // За последний год
                return violationsInspections
                    .Where(list => list.FirstOrDefault()?.Inspection.DatetimeCompleted >= today.AddYears(-1))
                    .ToList();

            default: // Все списки нарушений
                return violationsInspections.ToList();
        }
    }

    // Разделение функционала
    public List<IGrouping<int, ViolationInspection>> ApplyAccessControl(List<IGrouping<int, ViolationInspection>> violationsInspections, Employee employee)
    {
        /*
            2. Инспектор => Только свои
            3. Оператор => Все в своем департаменте
            */

        switch (employee.PostId)
        {
            case 2:
                return violationsInspections
                    .Where(list => list.FirstOrDefault()?.Inspection.InspectorId == employee.EmployeeId)
                    .ToList();

            case 3:
                return violationsInspections
                    .Where(list => list.FirstOrDefault()?.Inspection.Application.DepartmentId == employee.DepartmentId)
                    .ToList();

            default:
                return violationsInspections.ToList();
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _filterCB.SelectedIndex = 1;
        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = false;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }


    // Включает тригеры
    private void OnTriggers()
    {
        _filterCB.SelectionChanged += FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _filterCB.SelectionChanged -= FilterCB_SelectionChanged;
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class DepartmentDataService
{
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;

    public DepartmentDataService(ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB, 
        Button searchBTN, Button reserFiltersBTN, Action Action)
    {
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        UpdateIC = Action;

        sorterCB.ItemsSource = new[] { "По названию", "По адресу" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = false;

        OnTriggers();
    }

    // Поиск
    public List<Department> ApplySearch(List<Department> departments)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            departments = departments.Where(r =>
                r.Name.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Phone.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.Address.ToString().Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();
        }
        return departments;
    }

    // Сортировка
    public List<Department> ApplySort(List<Department> departments)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => departments.OrderBy(e => e.Address).ToList(),
                _ => departments.OrderBy(e => e.Name).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => departments.OrderByDescending(e => e.Address).ToList(),
                _ => departments.OrderByDescending(e => e.Name).ToList(),
            };
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = false;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }


    // Включает тригеры
    private void OnTriggers()
    {
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
    }
}

public class ReportDataService
{
    private ComboBox _sorterCB;
    private TextBox _searchTB;
    private CheckBox _ascendingCHB;
    private Button _reserFiltersBTN;
    private Button _searchBTN;
    private Action UpdateIC;
    private BindableDateBox _startDateTB;
    private BindableDateBox _endDateTB;

    public ReportDataService(ComboBox sorterCB, TextBox searchTB, CheckBox ascendingCHB,
        Button searchBTN, Button reserFiltersBTN, BindableDateBox startDateTB, BindableDateBox endDateTB, Action Action)
    {
        _sorterCB = sorterCB;
        _searchTB = searchTB;
        _ascendingCHB = ascendingCHB;
        _searchBTN = searchBTN;
        _reserFiltersBTN = reserFiltersBTN;
        _startDateTB = startDateTB;
        _endDateTB = endDateTB;
        UpdateIC = Action;

        sorterCB.ItemsSource = new[] { "По дате", "По статусу", "По ФИО владельца", "По инфо. ТС", "По департаменту" };
        sorterCB.SelectedIndex = 0;
        ascendingCHB.IsChecked = false;

        OnTriggers();
    }

    // Поиск
    public List<Report> ApplySearch(List<Report> reports)
    {
        string search = _searchTB.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            reports = reports.Where(r =>
                r.ApplcationId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.StatusName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.OwnerFullname.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.VehicleFullInfo.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.DepartmentName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.DatetimeSupply.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.DatetimeConfirm?.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) == true
                ).ToList();
        }
        return reports;
    }

    // Сортировка
    public List<Report> ApplySort(List<Report> reports)
    {
        int sortIndex = _sorterCB.SelectedIndex;
        bool ascending = (bool)_ascendingCHB.IsChecked;

        if (ascending)
        {
            return sortIndex switch
            {
                1 => reports.OrderBy(e => e.StatusName).ToList(),
                2 => reports.OrderBy(e => e.OwnerFullname).ToList(),
                3 => reports.OrderBy(e => e.VehicleFullInfo).ToList(),
                4 => reports.OrderBy(e => e.DepartmentName).ToList(),
                _ => reports.OrderBy(e => e.DatetimeSupply).ToList(),
            };
        }
        else
        {
            return sortIndex switch
            {
                1 => reports.OrderByDescending(e => e.StatusName).ToList(),
                2 => reports.OrderByDescending(e => e.OwnerFullname).ToList(),
                3 => reports.OrderByDescending(e => e.VehicleFullInfo).ToList(),
                4 => reports.OrderByDescending(e => e.DepartmentName).ToList(),
                _ => reports.OrderByDescending(e => e.DatetimeSupply).ToList(),
            };
        }
    }

    // Фильтрация по датам
    public List<Report> ApplyDateFilter(List<Report> reports)
    {
        bool isStartDateValid = DateOnly.TryParse(_startDateTB.DateText, out DateOnly startDate);
        bool isEndDateValid = DateOnly.TryParse(_endDateTB.DateText, out DateOnly endDate);

        if (isStartDateValid && isEndDateValid)
        {
            // Фильтруем заявки, у которых DatetimeSupply попадает в диапазон [startDate, endDate]
            return reports.Where(r =>
            {
                var supplyDate = DateOnly.FromDateTime(r.DatetimeSupply);
                return supplyDate >= startDate && supplyDate <= endDate;
            }).ToList();
        }
        else
        {
            // Если хотя бы одна дата невалидна — возвращаем все заявки без фильтрации
            return reports;
        }
    }

    // Обработчики событий
    private void AscendingCHB_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SorterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void SearchBTN_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void StartDateTB_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    private void EndDateTB_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender != null) UpdateIC();
    }

    // СБрос фильтров
    private void ResetFiltersBTN_Click(object sender, RoutedEventArgs e)
    {
        OffTriggers();

        _sorterCB.SelectedIndex = 0;
        _ascendingCHB.IsChecked = false;
        _searchTB.Clear();

        UpdateIC();
        OnTriggers();
    }

    // Включает тригеры
    private void OnTriggers()
    {
        _sorterCB.SelectionChanged += SorterCB_SelectionChanged;
        _ascendingCHB.Click += AscendingCHB_Click;
        _searchBTN.Click += SearchBTN_Click;
        _reserFiltersBTN.Click += ResetFiltersBTN_Click;
        _startDateTB.DateTextChangedExternal += StartDateTB_TextChanged;
        _endDateTB.DateTextChangedExternal += EndDateTB_TextChanged;
    }

    // Выключает тригеры
    private void OffTriggers()
    {
        _sorterCB.SelectionChanged -= SorterCB_SelectionChanged;
        _ascendingCHB.Click -= AscendingCHB_Click;
        _searchBTN.Click -= SearchBTN_Click;
        _reserFiltersBTN.Click -= ResetFiltersBTN_Click;
        _startDateTB.DateTextChangedExternal -= StartDateTB_TextChanged;
        _endDateTB.DateTextChangedExternal -= EndDateTB_TextChanged;
    }
}
