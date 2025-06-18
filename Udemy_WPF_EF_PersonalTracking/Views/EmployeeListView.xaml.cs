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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Udemy_WPF_EF_PersonalTracking.DB;
using Udemy_WPF_EF_PersonalTracking.ViewModels;

namespace Udemy_WPF_EF_PersonalTracking.Views
{
    /// <summary>
    /// Interaction logic for EmployeeListView.xaml
    /// </summary>
    public partial class EmployeeListView : UserControl
    {
        public EmployeeListView()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EmployeePage page = new EmployeePage();
            page.ShowDialog();
            FillGridEmployee();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        List<Position> positions = new List<Position>();
        List<EmployeeDetailModel> list = new List<EmployeeDetailModel>();
        EmployeeDetailModel model;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillGridEmployee();
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
            List<EmployeeDetailModel> searchList = list;
            if (txtUserNo.Text.Trim() != "")
                searchList = searchList.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                searchList = searchList.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                searchList = searchList.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                searchList = searchList.Where(x => x.PositionId == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                searchList = searchList.Where(x => x.DepartmentId == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            gridEmployee.ItemsSource = searchList;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtUserNo.Clear();
            txtSurname.Clear();
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.ItemsSource = positions;
            cmbPosition.SelectedIndex = -1;
            gridEmployee.ItemsSource = list;
        }

        private void FillGridEmployee()
        {
            cmbDepartment.ItemsSource = db.Departments.ToList();
            cmbDepartment.DisplayMemberPath = "DepartmentName";
            cmbDepartment.SelectedValuePath = "Id";
            cmbDepartment.SelectedIndex = -1;

            positions = db.Positions.ToList();

            cmbPosition.ItemsSource = db.Positions.ToList();
            cmbPosition.DisplayMemberPath = "PositionName";
            cmbPosition.SelectedValuePath = "Id";
            cmbPosition.SelectedIndex = -1;

            list = db.Employees.Include(x => x.Position).Include(x => x.Department).Select(x => new EmployeeDetailModel()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                BirthDay = ((DateOnly)x.BirtyDay).ToDateTime(TimeOnly.MinValue),
                DepartmentId = x.DepartmentId,
                DepartmentName = x.Department.DepartmentName,
                IsAdmin = (bool)x.IsAdmin,
                Password = x.Password,
                PositionId = x.PositionId,
                PositionName = x.Position.PositionName,
                Salary = x.Salary,
                Surname = x.Surname,
                UserNo = x.UserNo,
                ImagePath = x.ImagePath,
            }).ToList();
            gridEmployee.ItemsSource = list;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            EmployeePage page = new EmployeePage();
            page.model = model;
            page.ShowDialog();
            FillGridEmployee();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (model != null && model.Id != 0)
            {
                if (MessageBox.Show("Are you sure to delete?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // employee 삭제시 해당 employee와 연관된 직무, 급여, 허가 등의 관련 기록을 먼저 일괄 삭제.
                    // C# 코드에서 직접 삭제하는 해당 방법 이외 DBMS에서 Cascade, Trigger를 사용하는 방법도 존재.
                    List<DB.Task> tasksDelete = db.Tasks.Where(x => x.EmployeeId == model.Id).ToList();
                    foreach (DB.Task task in tasksDelete)
                        db.Tasks.Remove(task);
                    List<Permission> permissionsDelete = db.Permissions.Where(x => x.EmployeeId == model.Id).ToList();
                    foreach (Permission permission in permissionsDelete)
                        db.Permissions.Remove(permission);
                    List<Salary> salariesDelete = db.Salaries.Where(x => x.EmployeeId == model.Id).ToList();
                    foreach (Salary salary in salariesDelete)
                        db.Salaries.Remove(salary);
                    db.SaveChanges();

                    Employee employee = db.Employees.Find(model.Id);
                    db.Remove(employee);
                    db.SaveChanges();
                    MessageBox.Show($"{model.Name} employee was deleted.");
                    FillGridEmployee();
                }
            }
            else
                MessageBox.Show("Please select a employee from table.");
        }

        private void gridEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model = (EmployeeDetailModel)gridEmployee.SelectedItem;
        }
    }
}
