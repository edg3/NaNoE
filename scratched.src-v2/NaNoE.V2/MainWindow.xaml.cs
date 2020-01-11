using System;
using System.Collections.Generic;
using System.IO;
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

namespace NaNoE.V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NovelWebView.Initiate(webView);
            NovelWebView.Instance.RefreshView();
        }

        private void butNew_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = "SQLite Novel";
            sfd.DefaultExt = ".sqlite";
            sfd.Filter = "SQLite Novels (.sqlite)|*.sqlite";
            sfd.OverwritePrompt = false;

            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                if (File.Exists(sfd.FileName))
                {
                    MessageBox.Show("Can't use name of already created files.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    NovelDB.Create(sfd.FileName);
                }
            }
        }

        private void butOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "SQLite Novel";
            ofd.DefaultExt = ".sqlite";
            ofd.Filter = "SQLite Novels (.sqlite)|*.sqlite";

            Nullable<bool> result = ofd.ShowDialog();

            if (result == true)
            {
                NovelDB.Load(ofd.FileName);

                NovelWebView.Instance.RefreshView();
            }
        }
    }
}
