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

            var skatUserRepository = new SkatUserRepository { Session = session };
            
            //Entitäten erzeugen
            var skatuser = new SkatUser();
            skatuser.UserName = "Dieter";
            skatuser.UserValid = true;

            // Neuen Benutzer hinzufügen
            await skatUserRepository.AddNewSkatUserAsync(skatuser);
            
            await session.FlushAsync();
            await transaction.CommitAsync();
            
        }

        public void Dispose()
        {
            sessionFactory.Dispose();
        }
    }
}