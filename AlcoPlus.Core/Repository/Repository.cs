using AlcoPlus.Core.Contracts;
using AlcoPlus.Data;
using Microsoft.EntityFrameworkCore;

namespace AlcoPlus.Core.Repository;

// Generic Repository
public class Repository<T> : IRepository<T> where T : class
{
    private readonly AlcoPlusDbContext _context;

    public Repository(AlcoPlusDbContext alcoPlusDbContext)
    {
        _context = alcoPlusDbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);

        _context.Set<T>().Remove(entity);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);

        return entity is not null;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetAsync(int? id)
    {
        if (id is null)
        {
            return null;
        }

        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Update(entity);

        await _context.SaveChangesAsync();

        return entity;
    }
}
