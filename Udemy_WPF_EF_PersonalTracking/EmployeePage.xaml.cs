using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Window
    {
        public EmployeePage()
        {
            InitializeComponent();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        List<Position> positions = new List<Position>();
        public EmployeeDetailModel model;

        private void Window_Loaded(object sender, RoutedEventArgs e)
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

            if (model != null && model.Id != 0)
            {
                cmbDepartment.SelectedValue = model.DepartmentId;
                cmbPosition.SelectedValue = model.PositionId;
                txtUserNo.Text = model.UserNo.ToString();
                txtPassword.Text = model.Password;
                txtName.Text = model.Name;
                txtSurname.Text = model.Surname;
                txtSalary.Text = model.Salary.ToString();
                txtAddress.AppendText(model.Address);
                picker1.SelectedDate = model.BirthDay;
                chisAdmin.IsChecked = model.IsAdmin;
                txtImage.Text = model.ImagePath;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                // 현재 작업 디렉토리 + 상대경로(폴더 + 파일명). 상대경로는 에러로 인해 wpf에 이미지 안뜸
                image.UriSource = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", model.ImagePath), UriKind.Absolute);
                image.EndInit();
                EmployeeImage.Source = image;

                if (!UserStatic.IsAdmin)
                {
                    chisAdmin.IsEnabled = false;
                    txtUserNo.IsEnabled = false;
                    txtSalary.IsEnabled = false;
                    cmbDepartment.IsEnabled = false;
                    cmbPosition.IsEnabled = false;
                }
            }
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

        OpenFileDialog dialog = new OpenFileDialog();
        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            if (dialog.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(dialog.FileName);
                image.EndInit();
                EmployeeImage.Source = image;
                txtImage.Text = dialog.FileName;
            }
        }

        private void txtUserNo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserNo.Text.Trim() == "" || txtPassword.Text.Trim() == "" || txtName.Text.Trim() == ""
                || txtSurname.Text.Trim() == "" || txtSalary.Text.Trim() == "" || cmbDepartment.SelectedIndex == -1
                || cmbPosition.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill the necessary areas.");
            }
            else
            {
                if (model != null && model.Id != 0)
                {
                    Employee employee = db.Employees.Find(model.Id);

                    // UserNo 중복 확인. 번호 미변경시 생략
                    if (model.UserNo.ToString() != txtUserNo.Text)
                    {
                        List<Employee> employeeList = db.Employees.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text) && x.Id == employee.Id).ToList();
                        if (employeeList.Count > 0)
                        {
                            MessageBox.Show("This User no is already used by Another Employee.");
                            return;
                        }
                    }

                    // 이미지가 바뀌지 않을 경우에는 생략
                    if (txtImage.Text.Trim() != "" && txtImage.Text.Trim() != model.ImagePath)
                    {
                        if (File.Exists(@"Images//" + employee.ImagePath))
                        {
                            File.Delete(@"Images//" + employee.ImagePath);
                            string Unique = Guid.NewGuid().ToString();
                            StringBuilder filename = new StringBuilder(Unique + System.IO.Path.GetFileName(txtImage.Text));
                            File.Copy(txtImage.Text, @"Images//" + filename.ToString());
                            MessageBox.Show("Delete");
                        }
                    }
                    employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                    employee.Password = txtPassword.Text;
                    employee.Name = txtName.Text;
                    employee.Surname = txtSurname.Text;
                    employee.Salary = Convert.ToInt32(txtSalary.Text);
                    employee.DepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
                    employee.PositionId = Convert.ToInt32(cmbPosition.SelectedValue);
                    TextRange address = new TextRange(txtAddress.Document.ContentStart, txtAddress.Document.ContentEnd);
                    employee.Address = address.Text;
                    employee.BirtyDay = DateOnly.FromDateTime((DateTime)picker1.SelectedDate);
                    employee.IsAdmin = (bool)chisAdmin.IsChecked;
                    db.SaveChanges();
                    MessageBox.Show($"{model.Name} was Updated to {employee.Name}.");
                }
                else
                {
                    var Uniquelist = db.Employees.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
                    if (Uniquelist.Count > 0)
                    {
                        MessageBox.Show("This User No is Already used by another employee.");
                    }
                    else
                    {
                        Employee employee = new Employee();
                        employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                        employee.Password = txtPassword.Text;
                        employee.Name = txtName.Text;
                        employee.Surname = txtSurname.Text;
                        employee.Salary = Convert.ToInt32(txtSalary.Text);
                        employee.DepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
                        employee.PositionId = Convert.ToInt32(cmbPosition.SelectedValue);
                        TextRange address = new TextRange(txtAddress.Document.ContentStart, txtAddress.Document.ContentEnd);
                        employee.Address = address.Text;
                        employee.BirtyDay = DateOnly.FromDateTime((DateTime)picker1.SelectedDate);
                        employee.IsAdmin = (bool)chisAdmin.IsChecked;
                        string Unique = Guid.NewGuid().ToString();
                        StringBuilder filename = new StringBuilder(Unique + dialog.SafeFileName);
                        employee.ImagePath = filename.ToString();
                        db.Employees.Add(employee);
                        db.SaveChanges();

                        File.Copy(txtImage.Text, @"Images//" + filename.ToString());
                        MessageBox.Show($"{employee.Name} was Added.");

                        txtUserNo.Clear();
                        txtPassword.Clear();
                        txtName.Clear();
                        txtSurname.Clear();
                        txtSalary.Clear();
                        picker1.SelectedDate = DateTime.Today;
                        cmbDepartment.SelectedIndex = -1;
                        cmbPosition.SelectedIndex = -1;
                        cmbPosition.ItemsSource = positions;
                        txtAddress.Document.Blocks.Clear();
                        chisAdmin.IsChecked = false;
                        EmployeeImage.Source = new BitmapImage();
                        txtImage.Clear();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            bool isUnique = false;
            var Uniquelist = db.Employees.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (Uniquelist.Count > 0 )
            {
                MessageBox.Show("This User No is Already used by another employee.");
            }
            else
            {
                MessageBox.Show("This User No is available.");
            }    
        }
    }
}
