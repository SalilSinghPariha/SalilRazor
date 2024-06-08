using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //GET/ GET BY ID first or default/ ADD Record / Remove Record

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>,IOrderedQueryable<T>>? orderBy=null,
            string? includeProperites=null);

        T GetFirstOrDefault(Expression<Func<T, bool>>? filter =null, string? includeProperites = null);
    }

}
