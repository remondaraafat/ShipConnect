using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShipConnect.Data;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ShipConnectContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ShipConnectContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity is IBaseModel baseModel && baseModel.IsDeleted)
                return null;

            return entity;
        }

        public  IQueryable<T> GetAllAsync()
        {
            IQueryable<T> query = _dbSet;

            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            }

            return query;
        }

       
        public  IQueryable<T> GetWithFilterAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            }

            return query;
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);

            if (entity == null)
                throw new InvalidOperationException("Entity not found.");

            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                var prop = typeof(T).GetProperty("IsDeleted");
                if (prop != null)
                {
                    prop.SetValue(entity, true);
                    _dbSet.Update(entity);
                    return;
                }
            }

            _dbSet.Remove(entity);
        }


    }
}