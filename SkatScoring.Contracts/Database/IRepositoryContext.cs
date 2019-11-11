namespace SkatScoring.Contracts.Database
{
    public interface IRepositoryContext
    {
        ISkatUserRepository SkatUserRepository { get; }
    }
}