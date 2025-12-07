using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Spacifications
{
    public abstract class BaseSpacification<TEntity, Tkey> : ISpacifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        protected BaseSpacification(Expression<Func<TEntity, bool>> criteraExp)
        {
            Critera = criteraExp;
        }
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions {get;} = [];

        public Expression<Func<TEntity, bool>> Critera { get; }

        public Expression<Func<TEntity, object>> OrderBy { private set; get; }

        public Expression<Func<TEntity, object>> OrderByDesc { private set; get; }

        protected void  AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExpressions.Add(includeExp);
        }

        protected void AddOrderByAsc(Expression<Func<TEntity, object>> orderbyEx)
        {
            OrderBy = orderbyEx;
        }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderbyDescEx)
        {
            OrderByDesc = orderbyDescEx;
        }
    }
}
