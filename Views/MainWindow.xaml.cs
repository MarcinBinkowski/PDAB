
using System.Windows;

using PDAB.ViewModels;

namespace PDAB.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}