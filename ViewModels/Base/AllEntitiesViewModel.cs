using GalaSoft.MvvmLight.Messaging;
using PDAB.Helpers;
using PDAB.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace PDAB.ViewModels
{
    public abstract class AllEntitiesViewModel<T> : BaseWorkspaceViewModel where T : class
    {

        #region DB
        protected readonly PdabDbContext dbContext;
        #endregion
        

        #region Constructor
        public AllEntitiesViewModel(String displayName)
        {
            dbContext = new PdabDbContext();
            base.DisplayName = displayName;
        }
        #endregion

        public abstract void Load();
        
    }
}