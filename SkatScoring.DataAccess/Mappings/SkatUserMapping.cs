using FluentNHibernate.Mapping;
using SkatScoring.Contracts.Entities;

namespace SkatScoring.DataAccess.Mappings
{
    public sealed class SkatUserMapping : ClassMap<SkatUser>
    {
        public SkatUserMapping()
        {
            Table("skatuser");
            Id(x => x.UserId, "userid").GeneratedBy.GuidComb().Not.Nullable();
            Map(x => x.UserName, "username").Length(255).Not.Nullable();
            Map(x => x.UserValid, "uservalid").Not.Nullable();
        }
    }
}