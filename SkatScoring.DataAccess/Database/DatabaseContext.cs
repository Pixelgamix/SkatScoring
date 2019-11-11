using System;
using System.Threading.Tasks;
using SkatScoring.Contracts.Database;

namespace SkatScoring.DataAccess.Database
{
    public sealed class DatabaseContext : IDatabaseContext
    {
        private readonly DbContext _DbContext;
        private readonly RepositoryContext _RepositoryContext;

        public DatabaseContext(DbContext dbContext,
            RepositoryContext repositoryContext)
        {

            _DbContext = dbContext;
            _RepositoryContext = repositoryContext;
        }

        public async Task ExecuteAsync(Func<IRepositoryContext, Task> unitOfWork)
        {
            using var session = _DbContext.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            _RepositoryContext.Session = session;

            try
            {
                await unitOfWork(_RepositoryContext);
                await session.FlushAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                _RepositoryContext.Session = null;
                session.Dispose();
            }
            
        }
    }
}
