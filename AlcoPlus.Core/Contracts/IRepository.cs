using AlcoPlus.Core.Models;

﻿namespace AlcoPlus.Core.Contracts;

public interface IRepository<T> where T : class
{
    Task<T> GetAsync(int? id);

    Task<List<T>> GetAllAsync();
    
    Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);

    Task<T> AddAsync(T entity);

    Task DeleteAsync(int id);

    Task<T> UpdateAsync(T entity);

    Task<bool> Exists(int id);
}
