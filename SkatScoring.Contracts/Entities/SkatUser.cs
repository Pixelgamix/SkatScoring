using System;

namespace SkatScoring.Contracts.Entities
{
    public class SkatUser
    {
        public virtual Guid UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool UserValid { get; set; }
    }
}