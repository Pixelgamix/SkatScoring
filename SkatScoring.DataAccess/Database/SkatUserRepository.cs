using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using SkatScoring.Contracts.Database;
using SkatScoring.Contracts.Entities;

namespace SkatScoring.DataAccess.Database
{
    public sealed class SkatUserRepository : ISkatUserRepository
    {
        public ISession? Session;

        public Task AddNewSkatUserAsync(SkatUser skatuser)
        {
            if (Session is null) throw new InvalidOperationException(nameof(Session));
            return Session.SaveAsync(skatuser);
        }

        public Task<List<SkatUser>> ListSkatUserAsync()
        {
            if (Session is null) throw new InvalidOperationException(nameof(Session));
            return Session.Query<SkatUser>().ToListAsync();
        }

        public Task UpdateSkatUserAsync(SkatUser skatuser)
        {
            if (Session is null) throw new InvalidOperationException(nameof(Session));
            return Session.UpdateAsync(skatuser);
        }

        public Task GetSkatUserByName(string name)
        {
            if (Session is null) throw new InvalidOperationException(nameof(Session));
            return Session.Query<SkatUser>().Where(x => x.UserName == name).FirstOrDefaultAsync();
        }
    }
}