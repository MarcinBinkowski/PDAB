using PDAB.Models;

namespace PDAB.ViewModels;

public abstract class BusinessLogicViewModel : BaseWorkspaceViewModel
{
    protected readonly PdabDbContext dbContext;

    protected BusinessLogicViewModel()
    {
        dbContext = new PdabDbContext();
    }
}