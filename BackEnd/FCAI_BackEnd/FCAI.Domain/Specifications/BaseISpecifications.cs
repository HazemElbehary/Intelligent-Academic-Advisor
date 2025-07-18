using FCAI.Domain.Contracts;
using System.Linq.Expressions;

namespace FCAI.Domain.Specifications
{
    public class BaseISpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        // Where 'LINQ Operator'
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new();

        public BaseISpecifications(Expression<Func<TEntity, bool>>? Criteria)
        {
            this.Criteria = Criteria;
        }

        private protected virtual void AddIncludes()
        {
        }
    }
}
