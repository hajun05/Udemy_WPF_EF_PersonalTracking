using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Udemy_WPF_EF_PersonalTracking.DB;
using Udemy_WPF_EF_PersonalTracking.ViewModels;

namespace Udemy_WPF_EF_PersonalTracking.Views
{
    /// <summary>
    /// Interaction logic for SalaryList.xaml
    /// </summary>
    public partial class SalaryList : UserControl
    {
        public SalaryList()
        {
            InitializeComponent();
            FillSalaryGrid();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillSalaryGrid();
            cmbDepartment.ItemsSource = db.Departments.ToList();
            cmbDepartment.DisplayMemberPath = "DepartmentName";
            cmbDepartment.SelectedValuePath = "Id";
            cmbDepartment.SelectedIndex = -1;

            positions = db.Positions.ToList();
            cmbPosition.ItemsSource = db.Positions.ToList();
            cmbPosition.DisplayMemberPath = "PositionName";
            cmbPosition.SelectedValuePath = "Id";
            cmbPosition.SelectedIndex = -1;

            months = db.Salarymonths.ToList();
            cmbMonth.ItemsSource = months;
            cmbMonth.DisplayMemberPath = "MonthName";
            cmbMonth.SelectedValuePath = "Id";
            cmbMonth.SelectedIndex = -1;
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        List<SalaryDetailModel> salaries = new List<SalaryDetailModel>();
        //List<Employee> employeeList = new List<Employee>();
        List<Position> positions = new List<Position>();
        List<Salarymonth> months = new List<Salarymonth>();
        public SalaryDetailModel model = new SalaryDetailModel();
        int EmployeeId = 0;

        private void FillSalaryGrid()
        {
            salaries = db.Salaries.Include(x => x.Employee).Include(x => x.Month).Select(x => new SalaryDetailModel()
            {
                Id = x.Id,
                Name = x.Employee.Name,
                Surname = x.Employee.Surname,
                UserNo = x.Employee.UserNo,
                EmployeeId = x.EmployeeId,
                MonthId = x.MonthId,
                MonthName = x.Month.MonthName,
                Year = x.Year,
                Amount = x.Amount,
                DepartmentId = x.Employee.DepartmentId,
                PositionId = x.Employee.PositionId,
            }).OrderByDescending(x => x.Year).OrderByDescending(x => x.MonthId).ToList();
            gridSalary.ItemsSource = salaries;
        }
        private void txtNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            SalaryPage page = new SalaryPage();
            page.ShowDialog();
            FillSalaryGrid();
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int DepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
            if (cmbDepartment.SelectedIndex != -1)
            {
                cmbPosition.ItemsSource = positions.Where(x => x.DepartmentId == DepartmentId).ToList();
                cmbPosition.DisplayMemberPath = "PositionName";
                cmbPosition.SelectedValuePath = "Id";
                cmbPosition.SelectedIndex = -1;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<SalaryDetailModel> search = salaries;
            if (txtUserNo.Text.Trim() != "")
                search = search.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                search = search.Where(x => x.Name.ToLower().Contains(txtName.Text.ToLower())).ToList();
            if (txtSurname.Text.Trim() != "")
                search = search.Where(x => x.Surname.ToLower().Contains(txtSurname.Text.ToLower())).ToList();
            if (txtYear.Text.Trim() != "")
                search = search.Where(x => x.Year == Convert.ToInt32(txtYear.Text)).ToList();
            if (txtSalary.Text.Trim() != "")
            {
                if (rbMore.IsChecked == true)
                    search = search.Where(x => x.Amount > Convert.ToInt32(txtSalary.Text)).ToList();
                else if (rbLess.IsChecked == true)
                    search = search.Where(x => x.Amount < Convert.ToInt32(txtSalary.Text)).ToList();
                else
                    search = search.Where(x => x.Amount == Convert.ToInt32(txtSalary.Text)).ToList();
            }
            if (cmbMonth.SelectedIndex != -1)
                search = search.Where(x => x.MonthId == Convert.ToInt32(cmbMonth.SelectedValue)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                search = search.Where(x => x.PositionId == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                search = search.Where(x => x.DepartmentId == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            gridSalary.ItemsSource = search;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtUserNo.Clear();
            txtSurname.Clear();
            txtYear.Clear();
            txtSalary.Clear();
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.ItemsSource = positions;
            cmbPosition.SelectedIndex = -1;
            rbMore.IsChecked = false;
            rbLess.IsChecked = false;
            rbEquals.IsChecked = false;
            gridSalary.ItemsSource = salaries;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SalaryPage page = new SalaryPage();
            page.model = model;
            page.ShowDialog();
            FillSalaryGrid();
        }

        private void gridSalary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model = (SalaryDetailModel)gridSalary.SelectedItem;
            EmployeeId = model.EmployeeId;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gridSalary.SelectedIndex != -1 &&
                MessageBox.Show("Are you sure to delete?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (model.Id != 0)
                {
                    SalaryDetailModel salarymodel = (SalaryDetailModel)(gridSalary.SelectedItem);
                    Salary salary = db.Salaries.Find(salarymodel.Id);
                    db.Salaries.Remove(salary);

                    // 가장 최근에 지급된 급여를 Employee Page에 표시
                    var employee = db.Employees.Include(x => x.Salaries).FirstOrDefault(x => x.Id == EmployeeId);
                    employee.Salary = employee.Salaries.OrderByDescending(x => x.Year).ThenByDescending(x => x.MonthId).FirstOrDefault().Amount;
                    
                    db.SaveChanges();
                    MessageBox.Show($"{model.Name}'s {model.Year}-{model.MonthName} Salary was deleted.");
                    FillSalaryGrid();
                }
            }
        }
    }
}
