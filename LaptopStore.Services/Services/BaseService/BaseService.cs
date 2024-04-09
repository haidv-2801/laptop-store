﻿using LaptopStore.Core;
using LaptopStore.Core.Utilities;
using LaptopStore.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LaptopStore.Services.Services.BaseService
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly DbContext context;
        protected readonly DbSet<T> dbSet;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly Type _entityType;

        /// <summary>
        /// prefix code của phân hệ
        /// </summary>
        protected string _PrefixEntityCode = "Code";

        public BaseService(DbContext context, IHttpContextAccessor? httpContextAccessor)
        {
            this.context = context;
            dbSet = context.Set<T>();
            _entityType = typeof(T);
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<string> GetNextEntityCode(string propertyName)
        {
            int lastNumber = 0;
            var lastRow = await dbSet.AsNoTracking().OrderByDescending(e => e.CreatedDate).FirstOrDefaultAsync();
            if (lastRow != null)
            {
                PropertyInfo propertyInfo = lastRow.GetType().GetProperty(propertyName);
                // Kiểm tra xem property có tồn tại không
                if (propertyInfo != null)
                {
                    // Lấy giá trị của property
                    string value = (string)propertyInfo.GetValue(lastRow);
                    value = value.Substring(_PrefixEntityCode.Length);
                    lastNumber = Convert.ToInt32(value) + 1;
                }
            }
            else
            {
                lastNumber = 1;
            }
            string nextCode = $"{lastNumber}".PadLeft((7 - $"{lastNumber}".Length), '0');

            return _PrefixEntityCode + nextCode;
        }

        protected string GetUserLoginName()
        {
            var value = _httpContextAccessor.HttpContext.Request.Cookies["UserLogin"];
            if (value != null)
            {
                var account = JsonConvert.DeserializeObject<Account>(value);
                if (account != null)
                {
                    return account.FullName;
                }
            }
            return string.Empty;
        }

        protected bool HasProperty(string name)
        {
            return typeof(T).GetProperty(name) != null;
        }

        public async Task<T> AddEntityAsync(T entity)
        {
            SetTrackingAddProperties(entity);
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        private void SetTrackingAddProperties(T entity)
        {
            if (HasProperty("CreatedDate"))
                _entityType.GetProperty("CreatedDate")?.SetValue(entity, DateTime.Now);
            if (HasProperty("ModifiedDate"))
                _entityType.GetProperty("ModifiedDate")?.SetValue(entity, DateTime.Now);
            if (HasProperty("CreatedBy"))
                _entityType.GetProperty("CreatedBy")?.SetValue(entity, GetUserLoginName());
            if (HasProperty("ModifiedBy"))
                _entityType.GetProperty("ModifiedBy")?.SetValue(entity, GetUserLoginName());
        }

        public async Task<IEnumerable<T>> AddMultipleEntityAsync(IEnumerable<T> entities)
        {
            List<T> entityList = entities.ToList();
            var hasCreatedDateProperty = typeof(T).GetProperty("CreatedDate") != null;
            if (hasCreatedDateProperty)
            {
                for (int i = 0; i < entityList.Count(); i++)
                {
                    typeof(T).GetProperty("CreatedDate").SetValue(entityList[i], DateTime.Now);
                }
            }
            var hasCreatedByProperty = typeof(T).GetProperty("CreatedBy") != null;
            if (hasCreatedByProperty)
            {
                //_httpContextAccessor.HttpContext.Session.TryGetValue("UserLogin", out byte[] value);
                var value = _httpContextAccessor.HttpContext.Request.Cookies["UserLogin"];
                if (value != null)
                {
                    var account = JsonConvert.DeserializeObject<Account>(value);
                    for (int i = 0; i < entityList.Count(); i++)
                    {
                        typeof(T).GetProperty("CreatedBy").SetValue(entityList[i], account.FullName);
                    }
                }
            }
            await dbSet.AddRangeAsync(entityList);
            await context.SaveChangesAsync();
            return entities;
        }

        public async Task<int> UpdateEntityAsync(T entity)
        {
            var hasCreatedDateProperty = typeof(T).GetProperty("ModifiedDate") != null;
            if (hasCreatedDateProperty)
            {
                typeof(T).GetProperty("ModifiedDate").SetValue(entity, DateTime.Now);
            }
            var hasCreatedByProperty = typeof(T).GetProperty("ModifiedBy") != null;
            if (hasCreatedByProperty)
            {
                //_httpContextAccessor.HttpContext.Session.TryGetValue("UserLogin", out byte[] value);
                var value = _httpContextAccessor.HttpContext.Request.Cookies["UserLogin"];
                if (value != null)
                {
                    var account = JsonConvert.DeserializeObject<Account>(value);
                    typeof(T).GetProperty("ModifiedBy").SetValue(entity, account.FullName);
                }
                else
                {
                    typeof(T).GetProperty("ModifiedBy").SetValue(entity, "admin");
                }
            }
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
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<(IEnumerable<T> Data, int TotalRecords)> FilterEntitiesPagingAsync(
          Expression<Func<T, bool>> filter,
          string searchTerm,
          string searchFields, //fieldName1,fieldName2
          string sort, // "fieldName1:DESC,fieldName2:ASC"
          int pageNumber = 1,
          int pageSize = 10)
        {
            IQueryable<T> query = dbSet.AsNoTracking();

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
                        //var toStringCall = Expression.Call(propertyAccess, propertyAccess.Type.GetMethod("ToString", Type.EmptyTypes));
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var searchTermExpression = Expression.Constant(searchTerm, typeof(string));
                        var containsExpression = Expression.Call(propertyAccess, containsMethod, searchTermExpression);

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

            if (totalRecords == 0)
                return (Data: new List<T>(), TotalRecords: totalRecords);

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
            AsyncLocalLogger.Log("Tổng số record", totalRecords);
            return (Data: data, TotalRecords: totalRecords);
        }
    }
}
