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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Udemy_WPF_EF_PersonalTracking.DB;
using Udemy_WPF_EF_PersonalTracking.ViewModels;

namespace Udemy_WPF_EF_PersonalTracking.Views
{
    /// <summary>
    /// Interaction logic for PositionList.xaml
    /// </summary>
    public partial class PositionList : UserControl
    {
        public PositionList()
        {
            InitializeComponent();
            FillGridPos();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();

        private void FillGridPos()
        {
            var list = db.Positions.Include(x => x.Department).Include(x => x.Employees).Select(a => new
            {
                positionId = a.Id,
                positionName = a.PositionName,
                departmentId = a.DepartmentId,
                departmentName = a.Department.DepartmentName,
                // 각 Position별 배치된 직원 목록도 표기하도록 추가
                representative = string.Join(", ", a.Employees.Select(e => e.Name))
            }).OrderBy(x => x.positionName).ToList();

            List<PositionModel> modellist = new List<PositionModel>();
            foreach (var item in list)
            {
                PositionModel model = new PositionModel();
                model.Id = item.positionId;
                model.PositionName = item.positionName;
                model.DepartmentName = item.departmentName;
                model.DepartmentId = item.departmentId;
                model.Representative = item.representative;
                modellist.Add(model);
            }
            gridPosition.ItemsSource = modellist;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillGridPos();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PositionPage page = new PositionPage();
            page.ShowDialog();
            FillGridPos();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            PositionModel model = (PositionModel)gridPosition.SelectedItem;
            if (model != null && model.Id != 0)
            {
                PositionPage page = new PositionPage();
                page.model = model;
                page.ShowDialog();
                FillGridPos();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gridPosition.SelectedIndex != -1)
            {
                if (MessageBox.Show("Are you sure to delete?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    PositionModel model = (PositionModel)gridPosition.SelectedItem;
                    // employee 삭제시 해당 employee와 연관된 직무, 급여, 허가 등의 관련 기록을 먼저 일괄 삭제.
                    // C# 코드에서 직접 삭제하는 해당 방법 이외 DBMS에서 Cascade, Trigger를 사용하는 방법도 존재.
                    List<Employee> employeeDelete = db.Employees.Where(x => x.PositionId == model.Id).ToList();
                    foreach (Employee employee in employeeDelete)
                        db.Employees.Remove(employee);
                    db.SaveChanges();

                    Position position = db.Positions.Find(model.Id);
                    db.Positions.Remove(position);
                    db.SaveChanges();
                    MessageBox.Show($"{model.PositionName} position was deleted.");
                    FillGridPos();
                }
            }
            else
                MessageBox.Show("Please select a employee from table.");
        }
    }
}
