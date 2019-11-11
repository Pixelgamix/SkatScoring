using System.Collections.Generic;

namespace SkatScoring.Contracts.Database
{
    public sealed class DatabaseSettings
    {
        public IDictionary<string, string> Properties { get; set; }
    }
}