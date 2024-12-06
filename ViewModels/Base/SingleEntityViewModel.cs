using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;

public abstract class SingleEntityViewModel<T> : BaseWorkspaceViewModel
{
    #region DB
    protected PdabDbContext dbContext;
    #endregion

    #region Item
    protected T item;
    #endregion

    #region Command
    private BaseCommand _SaveCommand;
    public ICommand SaveCommand
    {
        get
        {
            if (_SaveCommand == null)
                _SaveCommand = new BaseCommand(() => SaveAndClose());
            return _SaveCommand;
        }
    }
    #endregion

    #region Constructor
    public SingleEntityViewModel(string displayName)
    {
        base.DisplayName = displayName;
        dbContext = new PdabDbContext();
    }
    #endregion

    #region Helpers
    public abstract void Save();
    protected virtual bool ValidateBeforeSave()
    {
        return true;
    }


    public void SaveAndClose()
    {
        if (ValidateBeforeSave())
        {
            Save();
            base.OnRequestClose();
        }
    }
    #endregion
}