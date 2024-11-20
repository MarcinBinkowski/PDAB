using PDAB.Models;

namespace PDAB.Repository;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly PdabDbContext _context;
    private readonly Dictionary<Type, object> _repositories;

    public RepositoryFactory(PdabDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        var type = typeof(T);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new Repository<T>(_context);
        }
        return (IRepository<T>)_repositories[type];
    }
}