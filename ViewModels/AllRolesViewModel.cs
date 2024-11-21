using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PDAB.Models;
using PDAB.Helpers;
using System.Windows.Input;
using PDAB.ViewModels;

namespace PDAB.ViewModels
{
    public class AllRolesViewModel : AllEntitiesViewModel<Role>
    {
       
        #region Constructor
        public AllRolesViewModel()
            :base("Roles")
        {
            Console.WriteLine("AllRolesViewModel()");
            Load();
        }
        #endregion
        #region Helpers
        public override void Load()
        {
            List=new ObservableCollection<Role>
            (
                dbContext.Roles.ToList()
            ); 
        }
        #endregion
    }
}