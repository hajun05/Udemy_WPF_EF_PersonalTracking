using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for PersmissionList.xaml
    /// </summary>
    public partial class PermissionList : UserControl
    {
        public PermissionList()
        {
            InitializeComponent();
            FillPermissionGrid();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        List<PermissionDetailModel> permissionlist = new List<PermissionDetailModel>();
        List<Position> positions = new List<Position>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillPermissionGrid();
            cmbDepartment.ItemsSource = db.Departments.ToList();
            cmbDepartment.DisplayMemberPath = "DepartmentName";
            cmbDepartment.SelectedValuePath = "Id";
            cmbDepartment.SelectedIndex = -1;

            positions = db.Positions.ToList();

            cmbPosition.ItemsSource = db.Positions.ToList();
            cmbPosition.DisplayMemberPath = "PositionName";
            cmbPosition.SelectedValuePath = "Id";
            cmbPosition.SelectedIndex = -1;

            List<Permissionstate> taskstates = db.Permissionstates.ToList();
            cmbState.ItemsSource = taskstates;
            cmbState.DisplayMemberPath = "StateName";
            cmbState.SelectedValuePath = "Id";
            cmbState.SelectedIndex = -1;
        }

        private void FillPermissionGrid()
        {
            permissionlist = db.Permissions.Include(x => x.Employee).Include(x => x.PermissionStateNavigation)
                .AsEnumerable().Select(x => new PermissionDetailModel() // AsEnumerable() // 이 시점부터는 LINQ-to-Objects로 동작. 쿼리의 SQL 변환이 불가능한 부분 메모리에서 처리
                {
                    Id = x.Id,
                    UserNo = x.Employee.UserNo,
                    EmployeeId = x.EmployeeId,
                    PositionId = x.Employee.PositionId,
                    DepartmentId = x.Employee.DepartmentId,
                    Name = x.Employee.Name,
                    Surname = x.Employee.Surname,
                    StateName = x.PermissionStateNavigation.StateName,
                    StartDate = (x.PermissionStartDate).ToDateTime(TimeOnly.MinValue),
                    EndDate = (x.PermissionEndDate).ToDateTime(TimeOnly.MinValue),
                    DayAmount = x.PermissionAmount,
                    Explanation = x.PermissionExplanation,
                    PermissionState = x.PermissionState,
                }).OrderByDescending(x => x.StartDate).ToList();
            
            gridPermission.ItemsSource = permissionlist;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PermissionPage page = new PermissionPage();
            page.ShowDialog();
            FillPermissionGrid();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<PermissionDetailModel> search = permissionlist;
            if (txtUserNo.Text.Trim() != "")
                search = search.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                search = search.Where(x => x.Name.ToLower().Contains(txtName.Text.ToLower())).ToList();
            if (txtSurname.Text.Trim() != "")
                search = search.Where(x => x.Surname.ToLower().Contains(txtSurname.Text.ToLower())).ToList();
            if (cmbPosition.SelectedIndex != -1)
                search = search.Where(x => x.PositionId == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                search = search.Where(x => x.DepartmentId == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            if (cmbState.SelectedIndex != -1)
                search = search.Where(x => x.DepartmentId == Convert.ToInt32(cmbState.SelectedValue)).ToList();
            if (rbStartDate.IsChecked == true)
                search = search.Where(x => x.StartDate > dpStart.SelectedDate && x.StartDate < dpEnd.SelectedDate).ToList();
            if (rbEndDate.IsChecked == true)
                search = search.Where(x => x.EndDate > dpStart.SelectedDate && x.EndDate < dpEnd.SelectedDate).ToList();
            if (txtDayAmount.Text.Trim() != "")
                search = search.Where(x => x.DayAmount == Convert.ToInt32(txtDayAmount.Text)).ToList();
            gridPermission.ItemsSource = search;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtUserNo.Clear();
            txtSurname.Clear();
            txtDayAmount.Clear();
            dpEnd.SelectedDate = DateTime.Today;
            dpStart.SelectedDate = DateTime.Today;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.ItemsSource = positions;
            cmbPosition.SelectedIndex = -1;
            rbEndDate.IsChecked = false;
            rbStartDate.IsChecked = false;
            gridPermission.ItemsSource = permissionlist;
        }
    }
}
