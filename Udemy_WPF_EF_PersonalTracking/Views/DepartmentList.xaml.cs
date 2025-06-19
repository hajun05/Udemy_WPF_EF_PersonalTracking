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
    /// Interaction logic for DepartmentList.xaml
    /// </summary>
    public partial class DepartmentList : UserControl
    {
        public DepartmentList()
        {
            InitializeComponent();
            FillGridDpt();
        }

        private void FillGridDpt()
        {
            using (PersonaltrackingContext db = new PersonaltrackingContext())
            {
                List<Department> list = db.Departments.OrderBy(x => x.DepartmentName).ToList();
                gridDepartment.ItemsSource = list;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DepartmentPage page = new DepartmentPage();
            page.ShowDialog();
            FillGridDpt();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Department dpt = (Department)gridDepartment.SelectedItem;
            DepartmentPage page = new DepartmentPage();
            page.department = dpt;
            page.ShowDialog();
            FillGridDpt();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gridDepartment.SelectedIndex != -1)
            {
                if (MessageBox.Show("Are you sure to delete?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using (PersonaltrackingContext db = new PersonaltrackingContext())
                    {
                        Department model = (Department)gridDepartment.SelectedItem;
                        // employee 삭제시 해당 employee와 연관된 직무, 급여, 허가 등의 관련 기록을 먼저 일괄 삭제.
                        // C# 코드에서 직접 삭제하는 해당 방법 이외 DBMS에서 Cascade, Trigger를 사용하는 방법도 존재.
                        List<Employee> employeeDelete = db.Employees.Where(x => x.DepartmentId == model.Id).ToList();
                        foreach (Employee employee in employeeDelete)
                            db.Employees.Remove(employee);
                        db.SaveChanges();

                        List<Position> positionDelete = db.Positions.Where(x => x.DepartmentId == model.Id).ToList();
                        foreach (Position position in positionDelete)
                            db.Positions.Remove(position);
                        db.SaveChanges();

                        Department department = db.Departments.Find(model.Id);
                        db.Departments.Remove(department);
                        db.SaveChanges();
                        MessageBox.Show($"{model.DepartmentName} department was deleted.");
                        FillGridDpt();
                    }
                }
                else
                    MessageBox.Show("Please select a department from table.");
            }
        }
    }
}
