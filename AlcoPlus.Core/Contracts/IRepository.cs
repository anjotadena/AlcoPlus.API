using AlcoPlus.Core.Models;

﻿namespace AlcoPlus.Core.Contracts;

public interface IRepository<T> where T : class
{
    Task<TResult> GetAsync<TResult>(int? id);

    Task<List<TResult>> GetAllAsync<TResult>();
    
    Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);

    Task<TResult> AddAsync<TSource, TResult>(TSource entity);

    Task DeleteAsync(int id);

    Task<TResult> UpdateAsync<TSource, TResult>(int id, TSource entity);

    Task<bool> Exists(int id);
}
