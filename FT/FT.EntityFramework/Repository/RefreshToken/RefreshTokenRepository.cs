using FT.Core.Security;
using FT.EntityFramework.EntityFramework.DbContext;
using FT.EntityFramework.EntityFramework.Repository;


namespace FT.EntityFramework.Repository
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
