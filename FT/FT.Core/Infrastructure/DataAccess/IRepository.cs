using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;

namespace FT.Core.Infrastructure.DataAccess
{
    public interface IUnitOfWork: IDisposable
    {
        ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot);
        Task CommitAsync(bool isSoftDelete, CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Repository</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
    public interface IRepository<T> : IUnitOfWork where T: class
    {
        Task AddAsync(T entity);
        Task DetachAsync(T entity);
        void AddRange(List<T> entity);
        void Update(T entity);
        void UpdateRange(List<T> itemList);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);

        Task<T> GetByIdAsync(int id);
        Task<T> GetByCompositeKey(int id1, int id2);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByIdASync(long id);
        //get entity without tracking

        Task<T> GetByIdAsNoTracking(int id);
        Task<T> GetByIdAsNoTracking(Guid id);
        Task<T> GetByIdAsNoTracking(Expression<Func<T,bool>> where,CancellationToken cancellationToken = default);
        void DeleteList(IQueryable<T> itemList);

        void DeleteRange(List<T> itemList);

        Task<List<T>> ToListAsync(CancellationToken cancellationToken = default);
        Task<List<T>> ToListAsync(Expression<Func<T,bool>> where,CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default); 
        
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }


        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<ICollection<TOutput>> ExecuteReaderAsync<TOutput>(string commandText, CommandType cmdType,
            SqlParameter[] parameters = null, CancellationToken cancellationToken = default) where TOutput : class;

        Task<List<T>> ToListAsNoTrackingAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default);
    }
}
