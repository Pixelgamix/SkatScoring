using System;
using System.Threading.Tasks;

namespace SkatScoring.Contracts.Database
{
    public interface IDatabaseContext
    {
        Task ExecuteAsync(Func<IRepositoryContext, Task> unitOfWork);
    }
}