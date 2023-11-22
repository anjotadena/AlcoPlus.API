using AutoMapper;
using AutoMapper.QueryableExtensions;
﻿using AlcoPlus.Core.Contracts;
using AlcoPlus.Data;
using AlcoPlus.Core.Models;
using Microsoft.EntityFrameworkCore;
using AlcoPlus.Core.Exceptions;

namespace AlcoPlus.Core.Repository;

// Generic Repository
public class Repository<T> : IRepository<T> where T : class
{
    private readonly AlcoPlusDbContext _context;
    private readonly IMapper _mapper;

    public Repository(AlcoPlusDbContext alcoPlusDbContext, IMapper mapper)
    {
        _context = alcoPlusDbContext;
        _mapper = mapper;
    }

    public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
    {
        var entity = _mapper.Map<T>(source);
        var result = await _context.AddAsync(entity);

        await _context.SaveChangesAsync();

        return _mapper.Map<TResult>(result.Entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync<T>(id);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, id);
        }

        _context.Set<T>().Remove(entity);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync<T>(id);

        return entity is not null;
    }

    public async Task<List<TResult>> GetAllAsync<TResult>()
    {
        return await _context.Set<T>()
                             .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                             .ToListAsync();
    }

    public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
    {
        var totalSize = await _context.Set<T>().CountAsync();
        var items = await _context.Set<T>()
                                  .Skip(queryParameters.StartIndex)
                                  .Take(queryParameters.PageSize)
                                  .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                                  .ToListAsync();

        return new PagedResult<TResult>
        {
            PageNumber = queryParameters.PageNumber,
            RecordNumber = queryParameters.PageSize,
            TotalCount = totalSize,
            Items = items,
        };
    }

    public async Task<TResult> GetAsync<TResult>(int? id)
    {
        var result = await _context.Set<T>().FindAsync(id);

        if (result is null)
        {
            throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key provided");
        }

        return _mapper.Map<TResult>(result);
    }

    public async Task<TResult> UpdateAsync<TSource, TResult>(int id, TSource source)
    {
        var entity = await GetAsync<T>(id);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, id);
        }

        entity = _mapper.Map(source, entity);

        _context.Update(entity);

        await _context.SaveChangesAsync();

        var result = _mapper.Map<TResult>(entity);

        return result;
    }
}
