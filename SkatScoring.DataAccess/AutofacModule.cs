using Autofac;
using SkatScoring.Contracts.Database;
using SkatScoring.DataAccess.Database;

namespace SkatScoring.DataAccess
{
    public sealed class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbContext>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<SkatUserRepository>()
                .AsSelf();

            builder.RegisterType<DatabaseContext>()
                .As<IDatabaseContext>();
            
            builder.RegisterType<RepositoryContext>()
                .AsSelf();
        }
    }
}
