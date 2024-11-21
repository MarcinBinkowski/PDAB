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
        private ObservableCollection<string> _Tables;
        private string _SelectedTable;
        private BaseWorkspaceViewModel _activeWorkspace;
        private ICommand _addNewItemCommand;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Console.WriteLine("Created MainWindowViewModel");
            Messenger.Default.Register<string>(this, open);
            _Commands = new ReadOnlyCollection<CommandViewModel>(CreateCommands());
            LoadTables();
        }
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
        
        public BaseWorkspaceViewModel ActiveWorkspace
        {
            get => _activeWorkspace;
            set
            {
                _activeWorkspace = value;
                OnPropertyChanged("ActiveWorkspace");
                (AddNewItemCommand as BaseCommand)?.RaiseCanExecuteChanged();
            }
        }
        public ICommand AddNewItemCommand
        {
            get
            {
                if (_addNewItemCommand == null)
                {
                    _addNewItemCommand = new BaseCommand(
                        () => AddNewItem(),
                        () => ActiveWorkspace != null);
                }
                return _addNewItemCommand;
            }
        }

        private List<CommandViewModel> CreateCommands()
        {
            Console.WriteLine("CreateCommands called");
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Roles",
                    new BaseCommand(() =>
                    {
                        this.ShowAllRoles();
                    })),
                new CommandViewModel(
                    "Categories",
                    new BaseCommand(() => this.ShowAllCategories()))
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

        
        
        #region Tables
        public ObservableCollection<string> Tables
        {
            get => _Tables;
            set
            {
                _Tables = value;
                OnPropertyChanged("Tables"); // Use string literal instead of lambda
            }
        }

        public string SelectedTable
        {
            get => _SelectedTable;
            set
            {
                _SelectedTable = value;
                OnPropertyChanged("SelectedTable"); 
            }
        }

        private void LoadTables()
        {
            Tables = new ObservableCollection<string>
            {
                "Roles"
            };
        }
        #endregion
        
        #region ShowTables
        private void ShowAllRoles()
        {
            Console.WriteLine("ShowAllRoles called");
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
        
        private void ShowAllCategories()
        {
            AllCategoriesViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllCategoriesViewModel) as AllCategoriesViewModel;
    
            if (workspace == null)
            {
                workspace = new AllCategoriesViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        
        private void AddNewRole()
        {
            NewRoleViewModel workspace = new NewRoleViewModel();
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }
        #endregion
        
        
        
        #region Private Helpers

        private void AddNewItem()
        {
            Console.WriteLine("AddNewItem called");
            if (ActiveWorkspace is AllRolesViewModel)
            {
                Console.WriteLine("AddNewItem for Roles called");
                CreateView(new NewRoleViewModel());
            }
            else if (ActiveWorkspace is AllCategoriesViewModel)
            {
                Console.WriteLine("AddNewItem for Categories called");
                CreateView(new NewCategoryViewModel());
            }
        }
        
        private void CreateView(BaseWorkspaceViewModel nowy)
        {
            this.Workspaces.Add(nowy); 
            this.SetActiveWorkspace(nowy); 
        }

        private void SetActiveWorkspace(BaseWorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));
            // re-evaluate CanExecute
            // (_addNewItemCommand as BaseCommand)?.RaiseCanExecuteChanged();

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
    }    
}
