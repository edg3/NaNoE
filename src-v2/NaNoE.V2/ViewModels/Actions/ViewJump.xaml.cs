using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ViewJump.xaml
    /// </summary>
    public partial class ViewJump : Window
    {
        public ViewJump()
        {
            InitializeComponent();
        }

        private void butChapter_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void butParagraph_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void butBookmark_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void butEnd_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void txtNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void butCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
