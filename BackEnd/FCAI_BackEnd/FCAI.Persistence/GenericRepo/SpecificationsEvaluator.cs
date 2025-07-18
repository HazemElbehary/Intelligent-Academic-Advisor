using FCAI.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace FCAI.Persistence.GenericRepo
{
    public static class SpecificationsEvaluator<TEntity, TKey>
       where TEntity : class
       where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, ISpecifications<TEntity, TKey> spec)
        {
            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            foreach (var Include in spec.Includes)
            {
                query.Include(Include);
            }

            return query;
        }
    }
}
