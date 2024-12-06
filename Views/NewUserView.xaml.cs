using System.Windows;
using System.Windows.Controls;
using PDAB.ViewModels;

namespace PDAB.Views;

public partial class NewUserView : UserControl
{
    public NewUserView()
    {
        InitializeComponent();
    }
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is NewUserViewModel viewModel)
        {
            viewModel.Password = ((PasswordBox)sender).Password;
        }
    }
}