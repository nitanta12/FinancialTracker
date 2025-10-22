using FT.Core.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.EntityFramework
{
    public class DbTransaction (IDbContextTransaction efTransaction) : ITransaction 
    {
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await efTransaction.CommitAsync(cancellationToken);
        }

        public async Task RollBackAsync(CancellationToken cancellationToken)
        {
            await efTransaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            efTransaction.Dispose();
        }

    }
}
