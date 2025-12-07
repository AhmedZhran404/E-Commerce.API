using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity , Tkey>(IQueryable<TEntity> entryPoint , ISpacifications<TEntity , Tkey> specifications) where TEntity : BaseEntity<Tkey>
        {
            var Query = entryPoint;

            if(specifications is not null)
            {

                if(specifications.Critera is not null)
                {
                    Query = Query.Where(specifications.Critera);
                }

                if(specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
                {
                    Query = specifications.IncludeExpressions.Aggregate
                            (
                               Query,
                               (CurrentQuery, includeExp) 
                                  => CurrentQuery.Include(includeExp)
                            );
                }
            }

            return Query;
        }
    }
}
