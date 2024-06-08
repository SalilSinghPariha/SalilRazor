using Microsoft.EntityFrameworkCore;
using Salil.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _dbContext;

        internal DbSet<T> _dbSet;
        public Repository(ApplicationDBContext applicationDBContext)
        {
            _dbContext = applicationDBContext;
            _dbSet = applicationDBContext.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>,IOrderedQueryable<T>>? orderBy=null,
            string? includeProperites = null)
        {
            IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperites != null)
            {
                foreach (var includeProperite in includeProperites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperite);
                }
            }

            if (orderBy != null)
            {
               return orderBy(query);
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperites = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

			if (includeProperites != null)
			{
				foreach (var includeProperite in includeProperites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProperite);
				}
			}
			return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet?.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}
