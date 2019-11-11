using NHibernate;
using SkatScoring.Contracts.Database;

namespace SkatScoring.DataAccess.Database
{
    public sealed class RepositoryContext : IRepositoryContext
    {
        public ISession? Session
        {
            get => session;
            set
            {
                if (value == Session) return;
                session = value;
                _skatUserRepository.Session = session;
            }
        }
        
        public ISkatUserRepository SkatUserRepository { get => _skatUserRepository; }

        private SkatUserRepository _skatUserRepository;
        private ISession? session;

        public RepositoryContext(SkatUserRepository skatUserRepository)
        {
            _skatUserRepository = skatUserRepository;
        }
    }
}