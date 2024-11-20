using System.Windows.Input;
using PDAB.Helpers;
using PDAB.ViewModels;

public abstract class BaseWorkspaceViewModel : BaseViewModel
{
    private BaseCommand _closeCommand;
    public ICommand CloseCommand => _closeCommand ??= new BaseCommand(OnRequestClose);
    public event EventHandler RequestClose;
    public BaseWorkspaceViewModel()
    {
    }
    public void OnRequestClose()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}