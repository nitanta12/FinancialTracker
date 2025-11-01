using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


using FT.EntityFramework.EntityFramework.Security.Models;
using FT.Core.Security;
using FT.Core.Domain.Common.Mail;
namespace FT.EntityFramework.EntityFramework.DbContext
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser, ApplicationIdentityRole, string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }


        #region DBSet
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<ResetTokens> ResetTokens { get; set; }
        #region MailSetting
        public DbSet<MailSetting> MailSettings { get; set; }
        #endregion

        #endregion
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            TrackAsync();

            await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task CommitAsync(bool isSoftDelete, CancellationToken cancellationToken)
        {
            TrackAsync(isSoftDelete);
            await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }


        private void TrackAsync(bool isSoftDelete = true)
        {
            ChangeTracker.DetectChanges();
            ChangeTracker.ProcessModification(UserId);

            if (isSoftDelete)
                ChangeTracker.ProcessDeletion(UserId);

            ChangeTracker.ProcessCreation(UserId, TenantId);
        }
    }
}
