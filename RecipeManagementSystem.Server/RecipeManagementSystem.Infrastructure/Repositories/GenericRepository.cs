using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RecipeManagementSystem.Domain;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Contracts;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Infrastructure.Data;
using RecipeManagementSystem.Shared.Errors;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Infrastructure.Repositories;

public class GenericRepository<T>(ApplicationDbContext context, IConfigurationProvider mapperConfig) : IGenericRepository<T>
    where T : BaseEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    
    public async Task<Result<PagedList<T>>> GetMembersAsync(FilterParams recipeParams)
    {
        var query = _dbSet?.AsQueryable();

        if (query == null)
        {
            return Result<PagedList<T>>.Success();
        }
            
        var entities = await PagedList<T>.CreateAsync(query, recipeParams.PageNumber, recipeParams.PageSize);

        return Result<PagedList<T>>.Success(entities);
    }

    public Result<IEnumerable<TDto>> GetAllByFilter<TDto>(Expression<Func<T, bool>> predicate)
    {
        var query = _dbSet.Where(predicate).AsNoTracking().ProjectTo<TDto>(mapperConfig).AsEnumerable();
        return Result<IEnumerable<TDto>>.Success(query);
    }

    public async Task<Result<T>> GetById(Guid id)
    {
        var data = await _dbSet.FindAsync(id);
        
        return data == null 
            ? Result<T>.Failure(EntityErrors.EntityNotFound) 
            : Result<T>.Success(data);
    }
    
    public async Task<Result> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        var result = await context.SaveChangesAsync();
        
        return result > 0
            ? Result.Success()
            : Result.Failure(EntityErrors.AddFailed);
    }

    public async Task<Result> UpdateAsync(T entity)
    {
        var existingEntity = await _dbSet.FindAsync(entity.Id);
        if (existingEntity is null)
        {
            return Result.Failure(EntityErrors.EntityNotFound);
        }

        var entry = context.Entry(existingEntity);
        foreach (var property in entry.Properties)
        {
            var propInfo = typeof(T).GetProperty(property.Metadata.Name);
            if (propInfo == null || !propInfo.CanWrite || property.Metadata.IsPrimaryKey())
            {
                continue;
            }

            var newValue = propInfo.GetValue(entity);
            var oldValue = property.CurrentValue;

            if (!Equals(newValue, oldValue))
            {
                property.CurrentValue = newValue;
            }
        }
        
        _dbSet.Update(entity);
        var result = await context.SaveChangesAsync();

        return result < 0 
            ? Result.Failure(EntityErrors.UpdateFailed) 
            : Result<T>.Success(entity);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null)
        {
            return Result.Failure(EntityErrors.EntityNotFound);
        }

        _dbSet.Remove(entity);

        var result = await context.SaveChangesAsync();

        return result < 0 
            ? Result.Failure(EntityErrors.DeleteFailed) 
            : Result<T>.Success(entity);
    }
}