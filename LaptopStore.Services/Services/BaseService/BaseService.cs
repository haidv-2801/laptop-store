﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Services.Services.BaseService
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<T> dbSet;

        public BaseService(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public async Task<T> AddEntityAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> UpdateEntityAsync(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteEntityAsync(T entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<T> GetEntityByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetEntitiesAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<(IEnumerable<T> Data, int TotalRecords)> FilterEntitiesPagingAsync(
          Expression<Func<T, bool>> filter,
          string searchTerm,
          string searchFields, //fieldName1,fieldName2
          string sort, // "fieldName1:DESC,fieldName2:ASC"
          int pageNumber = 1,
          int pageSize = 10)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchFields))
            {
                var fieldNames = searchFields.Split(',');
                var parameter = Expression.Parameter(typeof(T), "t");
                Expression searchExpression = null;
                foreach (var field in fieldNames)
                {
                    var propertyName = field.Trim();
                    var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                        var toStringCall = Expression.Call(propertyAccess, propertyAccess.Type.GetMethod("ToString", Type.EmptyTypes));
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var searchTermExpression = Expression.Constant(searchTerm, typeof(string));
                        var containsExpression = Expression.Call(toStringCall, containsMethod, searchTermExpression);

                        searchExpression = searchExpression == null ? containsExpression : Expression.OrElse(searchExpression, containsExpression);
                    }
                }
                if (searchExpression != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                    query = query.Where(lambda);
                }
            }

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(sort))
            {
                var sortExpressions = sort.Split(',');
                IOrderedQueryable<T> orderedQuery = null;
                foreach (var sortExpr in sortExpressions)
                {
                    var parts = sortExpr.Split(':');
                    if (parts.Length == 2)
                    {
                        var propertyName = parts[0].Trim();
                        var direction = parts[1].Trim().ToUpperInvariant();

                        var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                        if (property == null)
                            continue;

                        var parameter = Expression.Parameter(typeof(T), "x");
                        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                        var orderByExp = Expression.Lambda(propertyAccess, parameter);

                        orderedQuery = orderedQuery == null ?
                            (direction == "ASC" ?
                                Queryable.OrderBy(query, (dynamic)orderByExp) :
                                Queryable.OrderByDescending(query, (dynamic)orderByExp)) :
                            (direction == "ASC" ?
                                Queryable.ThenBy((dynamic)orderedQuery, (dynamic)orderByExp) :
                                Queryable.ThenByDescending((dynamic)orderedQuery, (dynamic)orderByExp));
                    }
                }
                query = orderedQuery ?? query;
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var data = await query.ToListAsync();

            return (Data: data, TotalRecords: totalRecords);
        }
    }
}
