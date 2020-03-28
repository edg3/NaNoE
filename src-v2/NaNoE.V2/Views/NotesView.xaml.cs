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
    /// Interaction logic for NotesView.xaml
    /// </summary>
    public partial class NotesView : Window
    {
        public NotesView()
        {
            InitializeComponent();
        }

        private void butBack_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.GotoLast();
        }

        private void lstElements_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
