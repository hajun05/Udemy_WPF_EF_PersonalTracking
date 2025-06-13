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

namespace Udemy_WPF_EF_PersonalTracking.Views
{
    /// <summary>
    /// Interaction logic for SalaryList.xaml
    /// </summary>
    public partial class SalaryList : UserControl
    {
        public SalaryList()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            SalaryPage page = new SalaryPage();
            page.ShowDialog();
        }
    }
}
