using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT.Core.Infrastructure.DataAccess
{
    public interface ITransaction
    {
        Task CommitAsync(CancellationToken cancellationToken);
        Task RollBackAsync(CancellationToken cancellationToken);
    }
}
