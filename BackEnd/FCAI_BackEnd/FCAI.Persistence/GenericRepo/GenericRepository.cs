using FCAI.Domain.Contracts;
using FCAI.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FCAI.Persistence.GenericRepo
{
    class GenericRepository<TEntity, TKey>(ApplicationDbContext dbContext) : IGenericRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
        {
            if(withTracking)
                return await dbContext.Set<TEntity>().ToListAsync();

            return await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec, bool withTracking = false)
        {
            return await SpecificationsEvaluator<TEntity, TKey>.GetQuery(dbContext.Set<TEntity>(), spec).ToListAsync();
        }

        public async Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec, bool withTracking = false)
        {
            var query = SpecificationsEvaluator<TEntity, TKey>
                            .GetQuery(dbContext.Set<TEntity>(), spec);

            if (!withTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetAsync(TKey Id)
        {
            return await dbContext.Set<TEntity>().FindAsync(Id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbContext.Set<TEntity>().AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }


        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }
    }
}
