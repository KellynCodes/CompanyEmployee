using System.Linq.Expressions;

namespace Repository.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        IQueryable<T> GetAll(bool tracking);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

}