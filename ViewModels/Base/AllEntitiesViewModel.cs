using GalaSoft.MvvmLight.Messaging;
using PDAB.Helpers;
using PDAB.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace PDAB.ViewModels
{
    public abstract class AllEntitiesViewModel<T> : BaseWorkspaceViewModel
    {
        #region DB
        protected readonly PdabDbContext dbContext;
        #endregion

        #region Command


        private BaseCommand _refreshCommand;
        public ICommand RefreshCommand // References in mainwindow.xaml
        {
            get
            {
                if (_refreshCommand == null)
                    _refreshCommand = new BaseCommand(() => Load()); // Load is implemented on all ViewModels
                return _refreshCommand;
            }
        }
        #endregion

        #region List
        private ObservableCollection<T> _List;
        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }
        #endregion

        #region Constructor
        public AllEntitiesViewModel(String displayName)
        {
            dbContext = new PdabDbContext();
            base.DisplayName = displayName;
        }
        #endregion

        #region Helpers
        public abstract void Load();

        #endregion
    }
}