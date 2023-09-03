using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Spv.Usuarios.DataAccess.Interface
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets objects from database with filtering and paging.
        /// </summary>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns></returns>
        bool Contains(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns></returns>
        T Find(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="trackEntity"></param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, bool trackEntity = true);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="entity">Specified a new object to create.</param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="entity">Specified a existing object to delete.</param>
        int Delete(T entity);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="entity">Specified the object to save.</param>
        /// <returns></returns>
        int Update(T entity);

        /// <summary>
        /// Get the total objects count.
        /// </summary>
        int Count { get; }

        void SaveChanges();

        IQueryable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        IQueryable<T> Includes(string includeProperties);

        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
    }

    /// <summary>
    /// Interface to support non-typed entities adding to a repository
    /// </summary>
    public interface IGenericRepository : IDisposable
    {
        void AddObject(object entity);

        void UpdateObject(object entity);
    }
}