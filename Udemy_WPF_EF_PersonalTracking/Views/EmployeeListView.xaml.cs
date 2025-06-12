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
            EmployeeDetailModel model = (EmployeeDetailModel)gridEmployee.SelectedItem;
            EmployeePage page = new EmployeePage();
            page.model = model;
            page.ShowDialog();
            FillGridEmployee();
        }
    }
}
