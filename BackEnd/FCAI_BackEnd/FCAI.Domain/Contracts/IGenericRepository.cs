namespace FCAI.Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey>
    where TEntity : class
    where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec, bool withTracking = false);
        Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec, bool withTracking = false);
        Task<TEntity?> GetAsync(TKey Id);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
