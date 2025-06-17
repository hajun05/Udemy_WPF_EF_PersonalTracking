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
        List<Position> positions = new List<Position>();
        public TaskDetailModel model = new TaskDetailModel();
        private int EmployeeId = 0;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridEmployee.ItemsSource = db.Employees.OrderBy(x => x.Name).ToList();

            cmbDepartment.ItemsSource = db.Departments.ToList();
            cmbDepartment.DisplayMemberPath = "DepartmentName";
            cmbDepartment.SelectedValuePath = "Id";
            cmbDepartment.SelectedIndex = -1;

            positions = db.Positions.ToList();
            cmbPosition.ItemsSource = db.Positions.ToList();
            cmbPosition.DisplayMemberPath = "PositionName";
            cmbPosition.SelectedValuePath = "Id";
            cmbPosition.SelectedIndex = -1;

            if (model != null && model.Id != 0)
            {
                txtUserNo.Text = model.UserNo.ToString();
                txtName.Text = model.Name;
                txtSurname.Text = model.Surname;
                txtContent.Text = model.TaskContent;
                txtTitle.Text = model.TaskTitle;
            }
        }
        
        private void gridEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee employee = (Employee)gridEmployee.SelectedItem;
            txtUserNo.Text = employee.UserNo.ToString();
            txtName.Text = employee.Name;
            txtSurname.Text = employee.Surname;
            // 추가 요소. Tasks는 Employee에 임의로 추가한 네비게이션 프로퍼티(외래키 X), include해야 정상 조회.
            // Where의 결과는 컬렉션 O, 객체 X. 특정 단일 요소(행)의 속성(컬럼 값)을 조회할때는 First 인스턴스 사용.
            txtCount.Text = db.Employees.Include(x => x.Tasks).FirstOrDefault(x => x.Id == employee.Id).Tasks.Count.ToString();
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
            if (txtTitle.Text.Trim() == "" || txtContent.Text.Trim() == "")
            {
                MessageBox.Show("Please fill the necessary areas.");
            }
            else
            {
                if (model != null && model.Id != 0)
                { 
                    DB.Task task = db.Tasks.Find(model.Id);
                    if (EmployeeId != 0)
                        task.EmployeeId = EmployeeId;
                    task.TaskTitle = txtTitle.Text;
                    task.TaskContent = txtContent.Text;
                    db.SaveChanges();
                    MessageBox.Show($"{model.TaskTitle} Task was Updated to {task.TaskTitle}.");
                }
                else
                {
                    if (EmployeeId == 0)
                    {
                        MessageBox.Show("Please select an employee from tabel.");
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
                    }
                }
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
