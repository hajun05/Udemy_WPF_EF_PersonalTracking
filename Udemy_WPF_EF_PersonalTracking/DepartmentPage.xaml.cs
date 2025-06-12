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
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : Window
    {
        public DepartmentPage()
        {
            InitializeComponent();
        }

        public Department department;

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtDepartmentName.Text.Trim() == "")
                MessageBox.Show("Please fill the Department Name area.");
            else
            {
                using (PersonaltrackingContext db = new PersonaltrackingContext())
                {
                    if (department != null && department.Id != 0)
                    {
                        Department update = new Department();
                        update.DepartmentName = txtDepartmentName.Text;
                        update.Id = department.Id;
                        db.Departments.Update(update);
                        db.SaveChanges();
                        MessageBox.Show($"{department.DepartmentName} Department was Updated to {update.DepartmentName}.");
                        txtDepartmentName.Clear();
                    }
                    else
                    {
                        Department dpt = new Department();
                        dpt.DepartmentName = txtDepartmentName.Text;
                        db.Departments.Add(dpt);
                        db.SaveChanges();
                        MessageBox.Show("Department was Added.", txtDepartmentName.Text);
                        txtDepartmentName.Clear();
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (department != null && department.Id != 0)
            {
                txtDepartmentName.Text = department.DepartmentName;
                txtDepartmentName.Focus();
                txtDepartmentName.Select(txtDepartmentName.Text.Length, 0);
            }
        }
    }
}
