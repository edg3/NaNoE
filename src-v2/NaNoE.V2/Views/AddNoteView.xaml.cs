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

namespace NaNoE.V2.Views
{
    /// <summary>
    /// Interaction logic for AddNoteView.xaml
    /// </summary>
    public partial class AddNoteView : Window
    {
        public AddNoteView()
        {
            InitializeComponent();
        }

        private void lstElements_Loaded(object sender, RoutedEventArgs e)
        {
            lstElements.ItemsSource = ViewModelLocator.Instance.NovelAddNoteVM.Models;
            lstElements.UpdateLayout();
        }
    }
}
