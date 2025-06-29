using Int.Backend.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Int.Backend.Servicios.Genericos.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<(IEnumerable<T> Items, int TotalCount)> GetAll(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            int pageNumber = 1,
            int pageSize = int.MaxValue);
        Task<T?> GetFirst(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task Delete(int id);
        Task DeleteMany(Expression<Func<T, bool>> filter);
        Task Update(int id, T newEntityToUpdate);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackTransactionAsync(IDbContextTransaction transaction);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<TResult>> ExecuteStoredProcedure<TResult>(string procedureName, params SqlParameter[] parameters);
        Task<IEnumerable<dynamic>> ExecuteStoredProcedureDynamic(string procedureName, params SqlParameter[] parameters);
        Task<int> ExecuteSqlRawAsync(string sql);
    }
}
