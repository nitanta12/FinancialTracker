using FT.Core.Security;
using FT.Core.Security.Infrastructure;
using FT.EntityFramework.EntityFramework.DbContext;
using FT.EntityFramework.EntityFramework.Repository;

namespace FT.EntityFramework.EntityFramework.Security.Repository
{
    public class ResetTokenRepository(ApplicationDbContext dbContext) : Repository<ResetTokens>(dbContext), IResetTokenRepository 
    {
    }
}
