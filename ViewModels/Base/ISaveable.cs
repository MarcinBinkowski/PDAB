namespace PDAB.ViewModels;

public interface ISaveable
{
    bool HasChanges { get; }
    Task SaveAsync();
}
