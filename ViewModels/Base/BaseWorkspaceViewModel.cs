using System.Windows.Input;
using PDAB.Helpers;
using PDAB.ViewModels;

public abstract class BaseWorkspaceViewModel : BaseViewModel, ISaveable
{
    public virtual bool HasChanges { get; protected set; }
    public abstract Task SaveAsync();
    private BaseCommand _CloseCommand;

    public BaseWorkspaceViewModel()
    {
    }

    public ICommand CloseCommand
    {
        get
        {
            if (_CloseCommand == null)
                _CloseCommand = new BaseCommand(() => this.OnRequestClose());
            return _CloseCommand;
        }
    }

    public event EventHandler RequestClose;
    public void OnRequestClose()
    {
        EventHandler handler = this.RequestClose;
        if (handler != null)
            handler(this, EventArgs.Empty);
    }
}