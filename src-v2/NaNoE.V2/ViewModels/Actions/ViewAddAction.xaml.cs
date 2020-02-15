using NaNoE.V2.Data;
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

namespace NaNoE.V2.ViewModels.Actions
{
    /// <summary>
    /// Interaction logic for ViewAddAction.xaml
    /// </summary>
    public partial class ViewAddAction : Window
    {
        public ViewAddAction()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.ViewAddActionVM.Window = this;
        }
    }
}
