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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NaNoE.V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _instance;
        public static MainWindow Instance
        {
            get { return _instance; }
        }

        public string DBCount
        {
            get
            {
                try
                {
                    return "words - " + DBManager.Instance.Count;
                }
                catch { }

                return "";
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            new CommandMap();
            new ViewModelLocator();
            new Navigator(frmNav);
            
            Navigator.Instance.Goto("start");

            _instance = this;
        }
    }
}
