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
    /// Interaction logic for TaskPage.xaml
    /// </summary>
    public partial class TaskPage : Window
    {
        public TaskPage()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        List<Employee> employeeList = new List<Employee>();
        List<Position> positions = new List<Position>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            employeeList = db.Employees.OrderBy(x => x.Name).ToList();
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
        }

        private int EmployeeId = 0;
        private void gridEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee employee = (Employee)gridEmployee.SelectedItem;
            txtUserNo.Text = employee.UserNo.ToString();
            txtName.Text = employee.Name;
            txtSurname.Text = employee.Surname;
            EmployeeId = employee.Id;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeId == 0)
            {
                MessageBox.Show("Please select an employee from tabel.");
            }
            else if (txtTitle.Text.Trim() == "" || txtContent.Text.Trim() == "")
            {
                MessageBox.Show("Please fill the necessary areas.");
            }
            else
            {
                DB.Task task = new DB.Task();
                task.EmployeeId = EmployeeId;
                task.TaskStartDate = DateOnly.FromDateTime((DateTime)DateTime.Now);
                task.TaskTitle = txtTitle.Text;
                task.TaskContent = txtContent.Text;
                task.TaskState = Definitions.TaskStates.OnEmployee;
                db.Tasks.Add(task);
                db.SaveChanges();
                MessageBox.Show($"{task.TaskTitle} Task was added.");

                EmployeeId = 0;
                txtContent.Clear();
                txtTitle.Clear();
                txtUserNo.Clear();
                txtName.Clear();
                txtSurname.Clear();

            }
        }
    }
}
