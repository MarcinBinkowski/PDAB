// MainWindow.xaml.cs
using System.Windows;
using PDAB.ViewModels;

namespace PDAB
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