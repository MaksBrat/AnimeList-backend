using AnimeList.DAL.Interfaces;
using AnimeList.Domain.RequestModels.SearchAnime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AnimeList.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public IList<TEntity> GetAll() => _dbSet.ToList();

        public async Task<IList<TEntity>> GetAllAsync() =>
            await _dbSet.ToListAsync();
       
        public async Task<IList<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? predicate = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;       

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }           

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }

            return orderBy is not null
                ? await orderBy(query).ToListAsync()
                : await query.ToListAsync();
        }

        public TEntity? GetFirstOrDefault(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return orderBy is not null
                ? orderBy(query).FirstOrDefault()
                : query.FirstOrDefault();
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>? predicate = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, 
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return orderBy is not null
                ? await orderBy(query).FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
        }

        public TResult? GetFirstOrDefault<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return orderBy is not null
                ? orderBy(query).Select(selector).FirstOrDefault()
                : query.Select(selector).FirstOrDefault();
        }

        public async Task<TResult?> GetFirstOrDefaultAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return orderBy is not null
                ? await orderBy(query).Select(selector).FirstOrDefaultAsync()
                : await query.Select(selector).FirstOrDefaultAsync();
        }

        public TEntity Insert(TEntity entity) => _dbSet.Add(entity).Entity;
        public void Insert(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

        public void Update(TEntity entity) => _dbSet.Update(entity);

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public void Delete(TEntity entity) => _dbSet.Remove(entity);
    }
}
