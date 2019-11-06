using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SkatScoring.Contracts.Entities;
using SkatScoring.DataAccess.Mappings;
using Xunit;

namespace SkatScoring.DataAccess.Tests
{
    public class SkatUserMappingTest : IDisposable
    {
        private ISessionFactory sessionFactory;

        public SkatUserMappingTest()
        {
            sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile("SkatUserMappingTest.sqlite"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SkatUserMapping>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false))
                .BuildSessionFactory();
        }

        [Fact]
        public void CreateAndSaveSkatUsers()
        {
            var session = sessionFactory.OpenSession();

            //Transaktion eröffnen
            using var transaction = session.BeginTransaction();

            //Entitäten erzeugen
            var skatuser = new SkatUser();
            skatuser.UserName = "Dieter";
            skatuser.UserValid = true;

            session.Save(skatuser);

            skatuser=new SkatUser();
            skatuser.UserName = "Frank";
            skatuser.UserValid = true;
            
            session.Save(skatuser);
            
            skatuser=new SkatUser();
            skatuser.UserName = "Kai";
            skatuser.UserValid = false;
            
            session.Save(skatuser);
            
            session.Flush();

            transaction.Commit();
        }

        public void Dispose()
        {
            sessionFactory.Dispose();
        }
    }
}