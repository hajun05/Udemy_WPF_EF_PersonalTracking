using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Udemy_WPF_EF_PersonalTracking.DB;
using Udemy_WPF_EF_PersonalTracking.ViewModels;

namespace Udemy_WPF_EF_PersonalTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoginName.Content = UserStatic.Name;
            IsAdmin.IsChecked = UserStatic.IsAdmin ? true : false;
            //btnEmployee.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            DataContext = new EmployeeViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (PersonaltrackingContext db = new PersonaltrackingContext())
            {
                if (!UserStatic.IsAdmin)
                {
                    stackDepartment.Visibility = Visibility.Hidden;
                    stackPosition.Visibility = Visibility.Hidden;
                    stackLogOut.SetValue(Grid.RowProperty, 5);
                    stackExit.SetValue(Grid.RowProperty, 6);
                }
            }
        }

        private void btnDepartment_Click(object sender, RoutedEventArgs e)
        {
            IblWindowName.Content = "Department List";
            DataContext = new DepartmentViewModel();
        }

        private void btnPosition_Click(object sender, RoutedEventArgs e)
        {
            IblWindowName.Content = "Position List";
            DataContext = new PositionViewModel();
        }

        private void btnEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (!UserStatic.IsAdmin)
            {
                using (PersonaltrackingContext db = new PersonaltrackingContext())
                {
                    Employee employee = db.Employees.Find(UserStatic.EmployeeId);
                    EmployeeDetailModel model = new EmployeeDetailModel();
                    model.Address = employee.Address;
                    model.BirthDay = ((DateOnly)employee.BirtyDay).ToDateTime(TimeOnly.MinValue);
                    model.DepartmentId = employee.DepartmentId;
                    model.Id = employee.Id;
                    model.ImagePath = employee.ImagePath;
                    model.IsAdmin = (bool)employee.IsAdmin;
                    model.Name = employee.Name;
                    model.Password = employee.Password;
                    model.PositionId = employee.PositionId;
                    model.Salary = employee.Salary;
                    model.Surname = employee.Surname;
                    model.UserNo = employee.UserNo;
                    EmployeePage page = new EmployeePage();
                    page.model = model;
                    page.ShowDialog();
                }
            }
            else 
            {
                IblWindowName.Content = "Employee List";
                DataContext = new EmployeeViewModel();
            }
        }

        private void btnTask_Click(object sender, RoutedEventArgs e)
        {
            IblWindowName.Content = "Task List";
            DataContext = new TaskViewModel();
        }

        private void btnSalary_Click(object sender, RoutedEventArgs e)
        {
            IblWindowName.Content = "Salary List";
            DataContext = new SalaryViewModel();
        }

        private void btnPermission_Click(object sender, RoutedEventArgs e)
        {
            IblWindowName.Content = "Permission List";
            DataContext = new PermissionViewModel();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}