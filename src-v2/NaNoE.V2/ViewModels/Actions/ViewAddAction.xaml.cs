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
        private bool _refresh;

        public ViewAddAction()
        {
            InitializeComponent();
            _refresh = true;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_refresh) return;
            ViewModelLocator.Instance.ViewAddActionVM.ID = ViewModelLocator.Instance.RunAddActionID;
            ViewModelLocator.Instance.ViewAddActionVM.Window = this;
            _refresh = false;

            // Refresh data context
            var source = grdBase.DataContext;
            grdBase.DataContext = null;
            grdBase.DataContext = source;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
