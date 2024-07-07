using System.Linq.Expressions;

namespace DataAccessLayer.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity item, CancellationToken cancellationToken = default);
        void Delete(TEntity item);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
