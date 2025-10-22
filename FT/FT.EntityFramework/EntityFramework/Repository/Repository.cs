using FT.Core.Infrastructure.DataAccess;
using FT.EntityFramework.EntityFramework.DbContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace FT.EntityFramework.EntityFramework.Repository
{
    public class Repository<T> : UnitOfWork, IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> Table
        {
            get
            {
                return _dbSet;
            }
        }

        public IQueryable<T> TableNoTracking
        {
            get
            {
                return _dbSet.AsNoTracking();
            }
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void AddRange(List<T> entity)
        {
           _dbSet.AddRange(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(where, cancellationToken).ConfigureAwait(false);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual  void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> obj = _dbSet.Where(where).AsEnumerable();
            foreach(T item in obj)
                _dbSet.Remove(item);
        }

        public virtual void DeleteList(IQueryable<T> itemList)
        {
            foreach (T obj in itemList)
                _dbSet.Remove(obj);
        }

        public virtual void DeleteRange(List<T> itemList)
        {
            _dbSet.RemoveRange(itemList);
        }

        public async Task DetachAsync(T entity)
        {
            await Task.Run(() =>
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            });
        }


        public async Task<ICollection<TOutput>> ExecuteReaderAsync<TOutput>(string commandText, CommandType cmdType, SqlParameter[] parameters = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            if(_dbContext.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                await _dbContext.Database.OpenConnectionAsync(cancellationToken: cancellationToken);
            }

            var command = _dbContext.Database.GetDbConnection().CreateCommand();

            if (_dbContext.Database.CurrentTransaction != null)
                command.Transaction = _dbContext.Database.CurrentTransaction.GetDbTransaction();

            command.CommandText = commandText;
            command.CommandType = cmdType;
            command.CommandTimeout = 500;

            if(parameters != null)
            {
                foreach(var param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }

            using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (!reader.HasRows)
                while (await reader.NextResultAsync(cancellationToken)) ;

            var mapper = new DataReaderMapper<TOutput>();
            return mapper.MapToList(reader);
        
        }


        public  async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbSet.Where(predicate);
            foreach (var include in includeProperties)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetByCompositeKey(int id1, int id2)
        {
            var entity =await _dbSet.FindAsync(id1,id2).ConfigureAwait(false);
            return entity;
        }

        public virtual async Task<T> GetByIdAsNoTracking(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<T> GetByIdAsNoTracking(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<T> GetByIdAsNoTracking(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(where, cancellationToken).ConfigureAwait(false);

            return entity;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<T> GetByIdASync(long id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }


        public virtual async Task<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(where).ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual async Task<List<T>> ToListAsNoTrackingAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().Where(where).ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        public  virtual void Update(T entity)
        {
           _dbSet.Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(List<T> itemList)
        {
            foreach(T obj in itemList)
            {
                _dbSet.UpdateRange(obj);
                _dbContext.Entry(obj).State = EntityState.Modified;
            }
        }
    }




    public class Repository<T, TContext> : UnitOfWork<TContext>, IRepository<T> where T : class where TContext : BaseDbContext
    {
        private readonly DbSet<T> _dbSet;

        public Repository(TContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> Table
        {
            get
            {
                return _dbSet;
            }
        }

        public IQueryable<T> TableNoTracking
        {
            get
            {
                return _dbSet.AsNoTracking();
            }
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void AddRange(List<T> entity)
        {
            _dbSet.AddRange(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(where, cancellationToken).ConfigureAwait(false);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> obj = _dbSet.Where(where).AsEnumerable();
            foreach (T item in obj)
                _dbSet.Remove(item);
        }

        public virtual void DeleteList(IQueryable<T> itemList)
        {
            foreach (T obj in itemList)
                _dbSet.Remove(obj);
        }

        public virtual void DeleteRange(List<T> itemList)
        {
            _dbSet.RemoveRange(itemList);
        }

        public async Task DetachAsync(T entity)
        {
            await Task.Run(() =>
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            });
        }


        public async Task<ICollection<TOutput>> ExecuteReaderAsync<TOutput>(string commandText, CommandType cmdType, SqlParameter[] parameters = null, CancellationToken cancellationToken = default) where TOutput : class
        {
            if (_dbContext.Database.GetDbConnection().State == ConnectionState.Closed)
            {
                await _dbContext.Database.OpenConnectionAsync(cancellationToken: cancellationToken);
            }

            var command = _dbContext.Database.GetDbConnection().CreateCommand();

            if (_dbContext.Database.CurrentTransaction != null)
                command.Transaction = _dbContext.Database.CurrentTransaction.GetDbTransaction();

            command.CommandText = commandText;
            command.CommandType = cmdType;
            command.CommandTimeout = 500;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }

            using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (!reader.HasRows)
                while (await reader.NextResultAsync(cancellationToken)) ;

            var mapper = new DataReaderMapper<TOutput>();
            return mapper.MapToList(reader);

        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbSet.Where(predicate);
            foreach (var include in includeProperties)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetByCompositeKey(int id1, int id2)
        {
            var entity = await _dbSet.FindAsync(id1, id2).ConfigureAwait(false);
            return entity;
        }

        public virtual async Task<T> GetByIdAsNoTracking(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<T> GetByIdAsNoTracking(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual async Task<T> GetByIdAsNoTracking(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(where, cancellationToken).ConfigureAwait(false);

            return entity;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<T> GetByIdASync(long id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }


        public virtual async Task<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> ToListAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(where).ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual async Task<List<T>> ToListAsNoTrackingAsync(Expression<Func<T, bool>> where, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().Where(where).ToListAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(List<T> itemList)
        {
            foreach (T obj in itemList)
            {
                _dbSet.UpdateRange(obj);
                _dbContext.Entry(obj).State = EntityState.Modified;
            }
        }
    }
}
