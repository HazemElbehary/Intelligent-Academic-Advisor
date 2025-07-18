namespace FCAI.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class
            where TKey : IEquatable<TKey>;
        Task<int> CompleteAsync();
    }
}
