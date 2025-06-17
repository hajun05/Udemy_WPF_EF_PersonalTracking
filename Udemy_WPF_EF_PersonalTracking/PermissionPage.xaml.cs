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

namespace Udemy_WPF_EF_PersonalTracking
{
    /// <summary>
    /// Interaction logic for PermissionPage.xaml
    /// </summary>
    public partial class PermissionPage : Window
    {
        public PermissionPage()
        {
            InitializeComponent();
        }

        TimeSpan tspermissionday = new TimeSpan();
        PersonaltrackingContext db = new PersonaltrackingContext();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserNo.Text = UserStatic.UserNo.ToString();
        }

        private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpStart.SelectedDate != null)
            {
                // 종료일은 시작일 이후만 선택 가능
                dpEnd.DisplayDateStart = dpStart.SelectedDate;
            }
            if (dpEnd.SelectedDate != null)
            {
                tspermissionday = (TimeSpan)(dpEnd.SelectedDate - dpStart.SelectedDate);
                txtDayAmount.Text = tspermissionday.TotalDays.ToString();
            }
        }

        private void dpEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpEnd.SelectedDate != null)
            {
                // 종료일은 시작일 이후만 선택 가능
                dpStart.DisplayDateEnd = dpEnd.SelectedDate;
            }
            if (dpStart.SelectedDate != null)
            {
                tspermissionday = (TimeSpan)(dpEnd.SelectedDate - dpStart.SelectedDate);
                txtDayAmount.Text = tspermissionday.TotalDays.ToString();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtDayAmount.Text.Trim() == "")
                MessageBox.Show("Please select start and end date.");
            else if (Convert.ToInt32(txtDayAmount.Text) <= 0)
                MessageBox.Show("Permission day must be bigger than zero.");
            else if (txtExplanation.Text.Trim() == "")
                MessageBox.Show("Please write your permission reason.");
            else
            {
                Permission permission = new Permission();
                permission.EmployeeId = UserStatic.EmployeeId;
                permission.UserNo = UserStatic.UserNo;
                permission.PermissionState = Definitions.PermissionStates.OnEmployee;
                permission.PermissionStartDate = DateOnly.FromDateTime((DateTime)dpStart.SelectedDate);
                permission.PermissionEndDate = DateOnly.FromDateTime((DateTime)dpEnd.SelectedDate);
                permission.PermissionAmount = Convert.ToInt32(txtDayAmount.Text);
                permission.PermissionExplanation = txtExplanation.Text;
                db.Permissions.Add(permission);
                db.SaveChanges();

                MessageBox.Show($"{UserStatic.Name}'s Permission was Added.");
                
                dpEnd.SelectedDate = DateTime.Today;
                dpStart.SelectedDate = DateTime.Today;
                txtExplanation.Clear();
                txtDayAmount.Clear();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
