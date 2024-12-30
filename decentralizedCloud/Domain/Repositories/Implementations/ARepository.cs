using System.Linq.Expressions;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Implementations;

public class ARepository<TEntity>:IRepository<TEntity>where TEntity:class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public ARepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity t)
    {
        var entity = await _dbSet.AddAsync(t);
        await _context.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task<List<TEntity>> CreateRangeAsync(List<TEntity> list)
    {
        await _dbSet.AddRangeAsync(list);
        await _context.SaveChangesAsync();
        return list;
    }

    public async Task<TEntity?> ReadAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.Where(filter).ToListAsync();
    }

    public async Task<List<TEntity>> ReadAsync(int start, int count)
    {
        return await _dbSet.Skip(start).Take(count).ToListAsync();
    }

    public async Task<List<TEntity>> ReadAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task UpdateAsync(TEntity t)
    {
        _dbSet.Update(t);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<TEntity> list)
    {
        _dbSet.UpdateRange(list);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity t)
    {
        _dbSet.Remove(t);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.CountAsync(filter);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.AnyAsync(filter);
    }
}