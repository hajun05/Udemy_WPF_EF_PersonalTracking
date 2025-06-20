﻿using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for TaskList.xaml
    /// </summary>
    public partial class TaskList : UserControl
    {
        public TaskList()
        {
            InitializeComponent();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        List<TaskDetailModel> tasklist = new List<TaskDetailModel>();
        List<TaskDetailModel> searchlist = new List<TaskDetailModel>();
        List<Position> positions = new List<Position>();
        TaskDetailModel model = new TaskDetailModel();

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            TaskPage page = new TaskPage();
            page.ShowDialog();
            FillTaskGrid();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillTaskGrid();

            if (!UserStatic.IsAdmin)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                btnApprove.SetValue(Grid.ColumnProperty, 1);
                btnApprove.Content = "Delivery";
            }
        }

        private void FillTaskGrid()
        {
            tasklist = db.Tasks.Include(x => x.TaskStateNavigation).Include(x => x.Employee)
                .ThenInclude(x => x.Department).ThenInclude(x => x.Positions).Select(x => new TaskDetailModel()
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Employee.Name,
                    StateName = x.TaskStateNavigation.StateName,
                    Surname = x.Employee.Surname,
                    TaskContent = x.TaskContent,
                    TaskStartDate = x.TaskStartDate == null ? DateTime.MinValue : ((DateOnly)x.TaskStartDate).ToDateTime(TimeOnly.MinValue),
                    TaskDeliveryDate = x.TaskDeliveryDate == null ? DateTime.MinValue : ((DateOnly)x.TaskDeliveryDate).ToDateTime(TimeOnly.MinValue),
                    TaskState = x.TaskState,
                    TaskTitle = x.TaskTitle,
                    UserNo = x.Employee.UserNo,
                    DepartmentId = x.Employee.DepartmentId,
                    PositionId = x.Employee.PositionId,
                }).ToList();

            if (!UserStatic.IsAdmin)
            {
                tasklist = tasklist.Where(x => x.EmployeeId == UserStatic.EmployeeId).ToList();
                txtName.IsEnabled = false;
                txtUserNo.IsEnabled = false;
                txtSurname.IsEnabled = false;
                cmbDepartment.IsEnabled = false;
                cmbPosition.IsEnabled = false;
            }

            gridTask.ItemsSource = tasklist;
            searchlist = tasklist;

            cmbDepartment.ItemsSource = db.Departments.ToList();
            cmbDepartment.DisplayMemberPath = "DepartmentName";
            cmbDepartment.SelectedValuePath = "Id";
            cmbDepartment.SelectedIndex = -1;

            positions = db.Positions.ToList();
            cmbPosition.ItemsSource = db.Positions.ToList();
            cmbPosition.DisplayMemberPath = "PositionName";
            cmbPosition.SelectedValuePath = "Id";
            cmbPosition.SelectedIndex = -1;

            List<Taskstate> taskstates = db.Taskstates.ToList();
            cmbState.ItemsSource = taskstates;
            cmbState.DisplayMemberPath = "NameState";
            cmbState.SelectedValuePath = "Id";
            cmbState.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<TaskDetailModel> search = searchlist;
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
            if (rbStart.IsChecked == true) 
                search = search.Where(x => x.TaskStartDate > dpStart.SelectedDate && x.TaskStartDate < dpDelivery.SelectedDate).ToList();
            if (rbDelivery.IsChecked == true)
                search = search.Where(x => x.TaskDeliveryDate > dpStart.SelectedDate && x.TaskDeliveryDate < dpDelivery.SelectedDate).ToList();
            gridTask.ItemsSource = search;
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtUserNo.Clear();
            txtSurname.Clear();
            dpDelivery.SelectedDate = DateTime.Today;
            dpStart.SelectedDate = DateTime.Today;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.ItemsSource = positions;
            cmbPosition.SelectedIndex = -1;
            rbDelivery.IsChecked = false;
            rbStart.IsChecked = false;
            gridTask.ItemsSource = tasklist;
        }

        private void gridTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model = (TaskDetailModel)gridTask.SelectedItem;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (model != null && model.Id != 0)
            {
                TaskPage page = new TaskPage();
                page.model = model;
                page.ShowDialog();
                FillTaskGrid();
            }
            else
                MessageBox.Show("Please select a task from table.");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gridTask.SelectedIndex != -1 &&
                MessageBox.Show("Are you sure to delete?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (model.Id != 0)
                {
                    TaskDetailModel taskmodel = (TaskDetailModel)(gridTask.SelectedItem);
                    DB.Task task = db.Tasks.Find(taskmodel.Id);
                    db.Tasks.Remove(task);

                    db.SaveChanges();
                    MessageBox.Show($"{model.Name}'s {model.TaskTitle} Task was deleted.");
                    FillTaskGrid();
                }
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (model != null && model.Id != 0)
            {
                if (UserStatic.IsAdmin && model.TaskState == Definitions.TaskStates.OnEmployee)
                    MessageBox.Show("Before approve a task, Task must be Delivered.");
                else if (model.TaskState == Definitions.TaskStates.Approved)
                    MessageBox.Show("This task is already approved.");
                else if (!UserStatic.IsAdmin && model.TaskState == Definitions.TaskStates.Delivered)
                    MessageBox.Show("This task is already delivered.");
                else
                {
                    DB.Task task = db.Tasks.Find(model.Id);
                    if (UserStatic.IsAdmin)
                        task.TaskState = Definitions.TaskStates.Approved;
                    else
                        task.TaskState = Definitions.TaskStates.Delivered;
                    db.SaveChanges();
                    string TaskUpdatedState = task.TaskState == Definitions.TaskStates.Approved ? "Approved" : "Delivered";
                    MessageBox.Show($"{model.TaskTitle} Task was {TaskUpdatedState}.");
                    FillTaskGrid();
                }
            }
        }
    }
}
