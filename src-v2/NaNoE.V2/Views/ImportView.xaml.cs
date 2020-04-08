using NaNoE.V2.Data;
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
using System.Windows.Shapes;

namespace NaNoE.V2.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : Window
    {
        public ImportView()
        {
            InitializeComponent();
        }

        private void butImportDocX_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog sfd = new Microsoft.Win32.OpenFileDialog();
            sfd.FileName = "DocX";
            sfd.DefaultExt = ".docx";
            sfd.Filter = "DocX (.docx)|*.docx";

            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                if (!File.Exists(sfd.FileName))
                {
                    MessageBox.Show("Can't use files that don't exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    lblImportDocX.Content = sfd.FileName;
                }
            }
        }

        private void butCreateSqlite_Click(object sender, RoutedEventArgs e)
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
                    lblCreateSqlite.Content = sfd.FileName;
                }
            }
        }

        private void butRunImport_Click(object sender, RoutedEventArgs e)
        {
            if (lblImportDocX.Content.ToString() == "")
            {
                MessageBox.Show("Please select a DocX to import.");
                return;
            }
            if (lblCreateSqlite.Content.ToString() == "")
            {
                MessageBox.Show("Please let us know the '.sqlite' to create.");
                return;
            }

            MessageBox.Show("Please note: this could be a long process if the word document is very long, just leave NaNoE running for it to complete.");

            throw new NotImplementedException();
        }

        private void butBack_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.Goto("start");
        }
    }
}
