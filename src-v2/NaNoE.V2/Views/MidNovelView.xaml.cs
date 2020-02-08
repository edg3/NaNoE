using NaNoE.V2.Data;
using NaNoE.V2.ViewModels;
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

namespace NaNoE.V2.Views
{
    /// <summary>
    /// Interaction logic for MidNovelView.xaml
    /// </summary>
    public partial class MidNovelView : Window
    {
        public MidNovelView()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModelLocator.Instance.MidNovelVM.View[0].ID == 1)
            {
                butUp.IsEnabled = false;
            }
            else
            {
                butUp.IsEnabled = true;
            }
        }
    }
}
