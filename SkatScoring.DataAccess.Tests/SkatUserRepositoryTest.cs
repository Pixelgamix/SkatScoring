using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SkatScoring.Contracts.Entities;
using SkatScoring.DataAccess.Database;
using SkatScoring.DataAccess.Mappings;
using Xunit;

namespace SkatScoring.DataAccess.Tests
{
    public class SkatUserRepositoryTest : IDisposable
    {
        private ISessionFactory sessionFactory;

        public SkatUserRepositoryTest()
        {
            sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile("SkatUserRepositoryTest.sqlite"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SkatUserMapping>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false))
                .BuildSessionFactory();
        }

        [Fact]
        public async void SkatUserRepositoryIntegration()
        {
            var session = sessionFactory.OpenSession();

            //Transaktion eröffnen
            using var transaction = session.BeginTransaction();

            var skatUserRepository = new SkatUserRepository {Session = session};

            //Entitäten erzeugen
            var skatuser1 = new SkatUser();
            skatuser1.UserName = "Dieter";
            skatuser1.UserValid = true;

            // Neuen Benutzer hinzufügen
            await skatUserRepository.AddNewSkatUserAsync(skatuser1);

            //Entitäten erzeugen
            var skatuser2 = new SkatUser();
            skatuser2.UserName = "Frank";
            skatuser2.UserValid = true;

            // weiteren Benutzer hinzufügen
            await skatUserRepository.AddNewSkatUserAsync(skatuser2);

            //Entitäten erzeugen
            var skatuser3 = new SkatUser();
            skatuser3.UserName = "Kai";
            skatuser3.UserValid = false;

            // weiteren Benutzer hinzufügen
            await skatUserRepository.AddNewSkatUserAsync(skatuser3);

            var skatuserlist = await skatUserRepository.ListSkatUserAsync();
            Assert.NotNull(skatuserlist);
            Assert.Equal(3, skatuserlist.Count);

            var skatuserDieter = skatUserRepository.GetSkatUserByNameAsync("Dieter");
            Assert.NotNull(skatuserDieter);
            Assert.True(skatuserDieter.Result.UserValid);

            var skatuserKai = skatUserRepository.GetSkatUserByNameAsync("Kai");
            Assert.NotNull(skatuserKai);
            Assert.False(skatuserKai.Result.UserValid);
            
            skatuserKai.Result.UserValid = true;
            await skatUserRepository.UpdateSkatUserAsync(skatuser3);
            
            skatuserKai = skatUserRepository.GetSkatUserByNameAsync("Kai");
            Assert.NotNull(skatuserKai);
            Assert.True(skatuserKai.Result.UserValid);
            
            await session.FlushAsync();
            await transaction.CommitAsync();
        }

        public void Dispose()
        {
            sessionFactory.Dispose();
        }
    }
}