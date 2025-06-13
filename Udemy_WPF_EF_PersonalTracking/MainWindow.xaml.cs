using System.Text;
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (PersonaltrackingContext context = new PersonaltrackingContext())
            {

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
            IblWindowName.Content = "Employee List";
            DataContext = new EmployeeViewModel();
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
    }
}