namespace PDAB.ViewModels
{
    public interface IDeletable
    {
        Task DeleteItemAsync(object item);
    }
}