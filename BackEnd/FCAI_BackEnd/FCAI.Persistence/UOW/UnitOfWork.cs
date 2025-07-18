using FCAI.Domain.Contracts;
using FCAI.Persistence.Data;
using FCAI.Persistence.GenericRepo;
using System.Collections.Concurrent;

namespace FCAI.Persistence.UOW
{
    class UnitOfWork: IUnitOfWork, IAsyncDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        ConcurrentDictionary<string, object> Repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
            Repositories = new();
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class
            where TKey : IEquatable<TKey>
        {
            return (GenericRepository<TEntity, TKey>)Repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_dbContext));
        }
    }
}
