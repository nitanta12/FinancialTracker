using FT.Core.Infrastructure.DataAccess;
using FT.EntityFramework.EntityFramework.DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.EntityFramework.EntityFramework.Repository
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        protected ApplicationDbContext _dbContext = dbContext;
        private Dictionary<Type, object> repositories;

        private readonly System.Transactions.TransactionOptions _options;



        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if(repositories == null)
            {
                repositories = new Dictionary<Type, object>();  
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(_dbContext);
            }

            return (IRepository<TEntity>) repositories[type];
        }


        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new DbTransaction(_dbContext.Database.CurrentTransaction ?? _dbContext.Database.BeginTransaction());
        }


        public async Task CommitAsync(bool isSoftDelete, CancellationToken cancellationToken = default)
        {
            await _dbContext.CommitAsync(isSoftDelete, cancellationToken).ConfigureAwait(false);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _dbContext.CommitAsync(cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }
    }


    public class UnitOfWork<TContext> : IUnitOfWork where TContext : BaseDbContext
    {
        protected TContext _dbContext;
        private Dictionary<Type, object> repositories;
        private readonly System.Transactions.TransactionOptions _options;

        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public IRepository<TEntity> GetRepository<TEntity>()
    where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity, TContext>(_dbContext);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new DbTransaction(_dbContext.Database.CurrentTransaction ?? _dbContext.Database.BeginTransaction());
        }
        public async Task CommitAsync(bool isSoftDelete, CancellationToken cancellationToken = default)
        {
            await _dbContext.CommitAsync(isSoftDelete, cancellationToken).ConfigureAwait(false);
        }
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _dbContext.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

    }
}
