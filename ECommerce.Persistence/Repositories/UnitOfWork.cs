using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;

        private readonly Dictionary<Type, object> _Repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var entityType = typeof(TEntity);
            if (_Repositories.TryGetValue(entityType, out var Repository))
            {
                return (IGenericRepository<TEntity, TKey>) Repository;
            }

            var newRepo = new GenericRepository<TEntity, TKey>(_dbContext);

            _Repositories[entityType] = newRepo;


            return newRepo;


        }

        public async Task<int> SaveChangeAsync()
        {
          return await _dbContext.SaveChangesAsync();
        }
    }
}
