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
        private BaseCommand _LoadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                    _LoadCommand = new BaseCommand(() => Load());
                return _LoadCommand;
            }
        }

        private BaseCommand _AddCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                    _AddCommand = new BaseCommand(() => add());
                return _AddCommand;
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
        private void add()
        {
            Messenger.Default.Send(DisplayName + "Add");
        }
        #endregion
    }
}