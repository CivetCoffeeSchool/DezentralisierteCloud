﻿using System.Linq.Expressions;

namespace Domain.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity:class
{
    Task<TEntity> CreateAsync(TEntity t);

    Task<List<TEntity>> CreateRangeAsync(List<TEntity> list);
    
    Task<TEntity?> ReadAsync(int id);

    Task<List<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> filter);

    Task<List<TEntity>>  ReadAsync(int start, int count);

    Task<List<TEntity>> ReadAllAsync();

    Task UpdateAsync(TEntity t);

    Task UpdateRangeAsync(List<TEntity> list);

    Task DeleteAsync(TEntity t);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> filter);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
}