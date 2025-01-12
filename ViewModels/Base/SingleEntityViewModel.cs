using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.ViewModels;

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
    public abstract bool Save();
    protected virtual bool ValidateBeforeSave()
    {
        return true;
    }


    public void SaveAndClose()
    {
        if (ValidateBeforeSave())
        {
            if(!Save())
            {
                MessageBox.Show("Error saving entity. Please check all required fields.",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            base.OnRequestClose();
        }
    }
    #endregion
}