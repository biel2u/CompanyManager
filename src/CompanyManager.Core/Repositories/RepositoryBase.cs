using CompanyManager.Core.Data;

namespace CompanyManager.Core.Repositories
{
    public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {
        protected ApplicationDbContext DbContext { get; }

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
