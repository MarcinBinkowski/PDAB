using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using PDAB.Helpers;

namespace PDAB.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
       #region Fields
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ObservableCollection<BaseWorkspaceViewModel> _Workspaces;
        #endregion

        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }
        private List<CommandViewModel> CreateCommands()
        {
            //
            Messenger.Default.Register<string>(this, open);
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Roles",
                    new BaseCommand(() => this.ShowAllRoles())),
            };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<BaseWorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<BaseWorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (BaseWorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (BaseWorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            BaseWorkspaceViewModel workspace = sender as BaseWorkspaceViewModel;
            this.Workspaces.Remove(workspace);
        }

        #endregion

        #region Private Helpers

        private void CreateView(BaseWorkspaceViewModel nowy)
        {
            this.Workspaces.Add(nowy); //to jest dodanie zakładki do kelkcji zakladek
            this.SetActiveWorkspace(nowy); //aktywowanie zakladki
        }

        private void SetActiveWorkspace(BaseWorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        private void open(string name)//name to jest ten wysłany komunikat
        {
            if (name == "RoleAdd")
                CreateView(new NewRoleViewModel());
        }
        #endregion
        
        #region ShowTables
        private void ShowAllRoles()
        {
            AllRolesViewModel workspace = 
                this.Workspaces.FirstOrDefault(vm => vm is AllRolesViewModel) 
                    as AllRolesViewModel;
            if (workspace == null)
            {
                workspace = new AllRolesViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }
        #endregion
    }    
}
