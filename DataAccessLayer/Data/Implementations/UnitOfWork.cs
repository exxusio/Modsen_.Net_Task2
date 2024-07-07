using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;

namespace DataAccessLayer.Data.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly Dictionary<Type, Func<AppDbContext, object>> _repositoryFactories;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositoryFactories = new Dictionary<Type, Func<AppDbContext, object>>
            {
                { typeof(User), ctx => new UserRepository(ctx) },
                { typeof(Role), ctx => new RoleRepository(ctx) },
                { typeof(Product), ctx => new ProductRepository(ctx) },
                { typeof(Category), ctx => new CategoryRepository(ctx) },
                { typeof(Order), ctx => new OrderRepository(ctx) },
                { typeof(OrderItem), ctx => new OrderItemRepository(ctx) }
            };
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositoryFactories.TryGetValue(typeof(TEntity), out var factory))
            {
                var repository = (IRepository<TEntity>)factory.Invoke(_dbContext);
                return repository;
            }
            else
                throw new InvalidOperationException($"No repository factory found for type {typeof(TEntity).Name}.");
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}