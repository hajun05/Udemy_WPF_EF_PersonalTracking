using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;
using Udemy_WPF_EF_PersonalTracking.DB;
using Udemy_WPF_EF_PersonalTracking.ViewModels;

namespace Udemy_WPF_EF_PersonalTracking
{
    /// <summary>
    /// Interaction logic for SalaryPage.xaml
    /// </summary>
    public partial class SalaryPage : Window
    {
        public SalaryPage()
        {
            InitializeComponent();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        List<Employee> employeeList = new List<Employee>();
        List<Position> positions = new List<Position>();
        List<Salarymonth> months = new List<Salarymonth>();
        public SalaryDetailModel model;
        int EmployeeId = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            employeeList = db.Employees.ToList();
            gridEmployee.ItemsSource = employeeList;

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

        private void gridEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee employee = (Employee)gridEmployee.SelectedItem;
            txtUserNo.Text = employee.UserNo.ToString();
            txtName.Text = employee.Name;
            txtSurname.Text = employee.Surname;
            txtSalary.Text = employee.Salary.ToString();
            txtYear.Text = DateTime.Now.Year.ToString();
            EmployeeId = employee.Id;
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridEmployee.ItemsSource = employeeList.Where(x => x.DepartmentId == Convert.ToInt32(cmbDepartment.SelectedItem)).ToList();
            int DepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
            if (cmbDepartment.SelectedIndex != -1)
            {
                cmbPosition.ItemsSource = positions.Where(x => x.DepartmentId == DepartmentId).ToList();
                cmbPosition.DisplayMemberPath = "PositionName";
                cmbPosition.SelectedValuePath = "Id";
                cmbPosition.SelectedIndex = -1;
            }
        }

        private void cmbPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridEmployee.ItemsSource = employeeList.Where(x => x.PositionId == Convert.ToInt32(cmbPosition.SelectedItem)).ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtSalary.Text.Trim() == "" || txtYear.Text.Trim() == "" || cmbMonth.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill the necessary areas.");
            }
            else
            {
                if (model != null && model.Id != 0)
                {

                }
                else
                {
                    if (EmployeeId == 0)
                    {
                        MessageBox.Show("Please select an employee from tabel.");
                    }
                    else
                    {
                        Salary salary = new Salary();
                        salary.EmployeeId = EmployeeId;
                        salary.Amount = Convert.ToInt32(txtSalary.Text);
                        salary.Year = Convert.ToInt32(txtYear.Text);
                        salary.MonthId = Convert.ToInt32(cmbMonth.SelectedValue);
                        db.Salaries.Add(salary);
                        db.SaveChanges();
                        MessageBox.Show($"{salary.Employee.Name}'s Salary was added.");
                    }

                    EmployeeId = 0;
                    txtSalary.Clear();
                    txtName.Clear();
                    txtUserNo.Clear();
                    txtSurname.Clear();
                    txtYear.Text = DateTime.Now.Year.ToString();
                    cmbMonth.SelectedIndex = -1;
                    gridEmployee.ItemsSource = employeeList;
                    cmbDepartment.SelectedIndex = -1;
                    cmbPosition.ItemsSource = positions;
                    cmbPosition.SelectedIndex = -1;
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
