using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using VezeetaProject.Core.Repository;

namespace VezeetaProject.EF.Repository
{
    public class RepositoryItem<T> : IBaseRepository<T> where T : class
    {
        private ApplicationDbContext _context;

        public RepositoryItem(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public async Task<T> GetbyIdAsync(int Id)
        {

            return await _context.Set<T>().FindAsync(Id);
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
          IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);
            
            return await query.SingleOrDefaultAsync(criteria);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {

            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.Where(criteria).CountAsync();
        }


        public async Task<IQueryable<IGrouping<Tkey,T>>> GroupBy<Tkey>(Expression<Func<T, Tkey>> Grouping,Expression<Func<T,bool>> criteria, string[] includes = null)
        {

            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);
           
            return  query.Where(criteria).GroupBy(Grouping);
        }
        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
       
            if (includes is not null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetTopByCondition(int Top, Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (includes is not null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return await query.OrderByDescending(criteria).Take(Top).ToListAsync();
        }


        public bool FindAny(Expression<Func<T, bool>> criteria, string[] includes = null) {
            
            IQueryable<T> query = _context.Set<T>();
            if (includes is not null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);
      
            return query.Any(criteria);
        }
        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> criteria, int Size, int Page, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);
            
            var Target = await query.Where(criteria).ToListAsync();

            return Pagination(Target,Size,Page);
        }
        private IEnumerable<T> Pagination(IEnumerable<T> Target, int Size = 10, int Page =1)
        {
            if (Size <= 0) Size = 10;
            if (Page <= 0) Size = 1;
            if (Target is not null)
                return Target.Skip((Page - 1) * Size).Take(Size).ToList();
            return null;      
        }
    }
}
