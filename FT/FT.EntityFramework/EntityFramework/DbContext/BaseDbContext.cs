using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace FT.EntityFramework.EntityFramework
{
    public class BaseDbContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
        {

        }
        protected BaseDbContext(DbContextOptions options)
           : base(options)
        {
        }
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            ChangeTracker.DetectChanges();
            await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual async Task CommitAsync(bool isSoftDelete, CancellationToken cancellationToken)
        {
            ChangeTracker.DetectChanges();
            await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
