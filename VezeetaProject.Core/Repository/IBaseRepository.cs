using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetbyIdAsync(int Id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> criteria, int Size, int Page, string[] includes = null);
        Task<IEnumerable<T>> GetTopByCondition(int Top, Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        bool FindAny(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IQueryable<IGrouping<Tkey, T>>> GroupBy<Tkey>(Expression<Func<T, Tkey>> Grouping, Expression<Func<T, bool>> criteria, string[] includes = null);
        


        }
    }
