using System.Linq.Expressions;
using RecipeManagementSystem.Domain.Common;
using RecipeManagementSystem.Domain.Entities;
using RecipeManagementSystem.Domain.Models;
using RecipeManagementSystem.Shared.Models;

namespace RecipeManagementSystem.Domain.Contracts;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<Result<PagedList<T>>> GetMembersAsync(FilterParams recipeParams);

    Result<IEnumerable<TDto>> GetAllByFilter<TDto>(Expression<Func<T, bool>> predicate);

    Task<Result<T>> GetById(Guid id);

    Task<Result> AddAsync(T entity);

    Task<Result> UpdateAsync(T entity);

    Task<Result> DeleteAsync(Guid id);
}