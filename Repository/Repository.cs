using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly PdabDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(PdabDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<ObservableCollection<T>> GetAllAsync()
    {
        var list = await _dbSet.ToListAsync();
        return new ObservableCollection<T>(list);
    }
    public async Task<IEnumerable<T>> GetAllIncludingAsync(Func<IQueryable<T>, IQueryable<T>> include)
    {
        var query = include(_context.Set<T>());
        return await query.ToListAsync();
    }
    public async Task<T> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync(); 
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public IQueryable<T> Query()
        => _dbSet.AsQueryable();
}