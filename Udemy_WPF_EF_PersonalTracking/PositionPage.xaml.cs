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
    /// Interaction logic for PositionPage.xaml
    /// </summary>
    public partial class PositionPage : Window
    {
        public PositionPage()
        {
            InitializeComponent();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        public PositionModel model;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = db.Departments.ToList().OrderBy(x => x.DepartmentName);

            cmbDepartment.ItemsSource = list;
            cmbDepartment.DisplayMemberPath = "DepartmentName";
            cmbDepartment.SelectedValuePath = "Id";
            cmbDepartment.SelectedIndex = -1;
            if (model != null && model.Id !=0)
            {
                cmbDepartment.SelectedValue = model.DepartmentId;
                txtPositionName.Text = model.PositionName;
                txtPositionName.Focus();
                txtPositionName.Select(txtPositionName.Text.Length, 0);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDepartment.SelectedIndex == -1 || txtPositionName.Text.Trim() == "")
                MessageBox.Show("Please fill all areas.");
            else
            {
                if (model != null && model.Id != 0)
                {
                    
                    Position position = new Position();
                    position.DepartmentId = (int)cmbDepartment.SelectedValue;
                    position.Id = model.Id;
                    position.PositionName = txtPositionName.Text;
                    db.Positions.Update(position);
                    db.SaveChanges();
                    cmbDepartment.SelectedIndex = -1;
                    txtPositionName.Clear();
                    MessageBox.Show($"{model.PositionName} Position was Updated to {position.PositionName}.");
                }
                else
                {
                    Position position = new Position();
                    position.PositionName = txtPositionName.Text;
                    position.DepartmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
                    db.Positions.Add(position);
                    db.SaveChanges();
                    cmbDepartment.SelectedIndex = -1;
                    txtPositionName.Clear();
                    MessageBox.Show($"{position.PositionName} Position was Added.");
                }
            }
        }
    }
}
