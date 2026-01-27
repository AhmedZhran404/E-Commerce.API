using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity)
        {
           await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CountAsync(ISpacifications<TEntity, Tkey> specifications)
        {

            var Query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>() , specifications);
            return await Query.CountAsync();
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
          return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpacifications<TEntity, Tkey> specifications)
        {
            var Query = SpecificationEvaluator.CreateQuery(
                _dbContext.Set<TEntity>(),
                specifications
            );

            return await Query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id)
        {
          return  await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetByIdAsync(ISpacifications<TEntity, Tkey> specifications)
        {
            var Query = SpecificationEvaluator.CreateQuery(
                _dbContext.Set<TEntity>(),
                specifications
            );

            return await Query.FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }
    }
}
