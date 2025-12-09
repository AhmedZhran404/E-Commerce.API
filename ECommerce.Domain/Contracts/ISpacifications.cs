using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ISpacifications<TEntity , Tkey> where TEntity : BaseEntity<Tkey>
    {
        ICollection<Expression<Func<TEntity , object>>> IncludeExpressions { get; }

        Expression<Func<TEntity , bool>> Critera { get; }

        public Expression<Func<TEntity , object>> OrderBy { get; }
        public Expression<Func<TEntity , object>> OrderByDesc { get; }

        public int Take { get;}

        public int Skip { get; }

        public bool IsPaginated { get;}

    }
}
