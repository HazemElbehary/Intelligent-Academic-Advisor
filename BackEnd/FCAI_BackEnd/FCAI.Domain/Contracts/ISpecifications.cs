using System.Linq.Expressions;

namespace FCAI.Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        // Where [LINQ Operator]
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
    }
}
