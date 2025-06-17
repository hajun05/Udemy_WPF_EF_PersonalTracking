using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for PersmissionList.xaml
    /// </summary>
    public partial class PermissionList : UserControl
    {
        public PermissionList()
        {
            InitializeComponent();
        }

        PersonaltrackingContext db = new PersonaltrackingContext();
        List<PermissionDetailModel> permissionlist = new List<PermissionDetailModel>();

        private void FillPermissionGrid()
        {
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PermissionPage page = new PermissionPage();
            page.ShowDialog();
        }
    }
}
