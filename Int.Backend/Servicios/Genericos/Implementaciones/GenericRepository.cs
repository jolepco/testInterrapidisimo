using Int.Backend.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Int.Backend.Servicios.Genericos.Implementaciones
{
    public class GenericRepository<T>(AppDataContext context) where T : class
    {
        protected AppDataContext context = context;
        private readonly DbSet<T> dbSet = context.Set<T>();

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetAll(
           Expression<Func<T, bool>>? filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
           Func<IQueryable<T>, IQueryable<T>>? include = null,
           int pageNumber = 1,
           int pageSize = int.MaxValue)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            query = query.AsNoTracking();

            var totalRecords = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var items = await query.ToListAsync();

            return (items, totalRecords);
        }

        public async Task<T?> GetFirst(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = dbSet;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(filter);
        }

        public virtual async Task AddAsync(T entity) =>
            await dbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) =>
            await dbSet.AddRangeAsync(entities);

        public virtual async Task Delete(int id)
        {
            T? entityToDelete = await dbSet.FindAsync([id]);

            if (entityToDelete != null)
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                    dbSet.Attach(entityToDelete);

                dbSet.Remove(entityToDelete);
            }
        }

        public virtual async Task DeleteMany(Expression<Func<T, bool>> filter)
        {
            var entitiesToDelete = await dbSet.Where(filter).ToListAsync();

            if (entitiesToDelete != null && entitiesToDelete.Any())
            {
                foreach (var entity in entitiesToDelete)
                {
                    if (context.Entry(entity).State == EntityState.Detached)
                        dbSet.Attach(entity);

                    dbSet.Remove(entity);
                }
            }
        }

        public virtual async Task Update(int id, T newEntityToUpdate)
        {
            T? entityToUpdate = await dbSet.FindAsync([id]);

            if (entityToUpdate != null)
            {
                context.Entry(entityToUpdate).CurrentValues.SetValues(newEntityToUpdate);
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync() =>
            await context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync(IDbContextTransaction transaction) =>
            await transaction.CommitAsync();

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction) =>
            await transaction.RollbackAsync();

        public virtual async Task<bool> SaveChangesAsync() =>
            (await context.SaveChangesAsync()) > 0;

        public async Task<IEnumerable<TResult>> ExecuteStoredProcedure<TResult>(string procedureName, params SqlParameter[] parameters)
        {
            await using var command = context.Database.GetDbConnection().CreateCommand();
            if (command.Connection == null)
                throw new InvalidOperationException("Database connection is not available");

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procedureName;
            command.CommandTimeout = 300;

            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            await command.Connection.OpenAsync();

            using var reader = (SqlDataReader)await command.ExecuteReaderAsync();
            var mapper = new ObjectMapper();
            return mapper.Map<TResult>(reader);

        }

        public async Task<IEnumerable<dynamic>> ExecuteStoredProcedureDynamic(
            string procedureName,
            params SqlParameter[] parameters)
        {
            await using var command = context.Database.GetDbConnection().CreateCommand();
            if (command.Connection == null)
                throw new InvalidOperationException("Database connection is not available");

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procedureName;
            command.CommandTimeout = 300;

            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            await command.Connection.OpenAsync();
            using var reader = (SqlDataReader)await command.ExecuteReaderAsync();

            var results = new List<dynamic>();
            while (await reader.ReadAsync())
            {
                dynamic obj = new ExpandoObject();
                var dict = (IDictionary<string, object>)obj;

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    dict[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                results.Add(obj);
            }
            return results;
        }

        public async Task<int> ExecuteSqlRawAsync(string sql)
        {
            return await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
    public class ObjectMapper
    {
        public IEnumerable<TResult> Map<TResult>(SqlDataReader reader)
        {
            var results = new List<TResult>();
            while (reader.Read())
            {
                var result = Activator.CreateInstance<TResult>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var propertyName = reader.GetName(i);
                    var propertyInfo = result?.GetType().GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        var value = reader.GetValue(i);
                        if (value == DBNull.Value)
                        {
                            if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
                                propertyInfo.SetValue(result, null);
                        }
                        else
                            propertyInfo.SetValue(result, value);
                    }
                }
                results.Add(result);
            }
            return results;
        }
    }
}
