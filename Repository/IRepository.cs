using System.Collections.ObjectModel;

public interface IRepository<T> where T : class
{
    Task<ObservableCollection<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllIncludingAsync(Func<IQueryable<T>, IQueryable<T>> include);
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SaveChangesAsync();
    IQueryable<T> Query();
}