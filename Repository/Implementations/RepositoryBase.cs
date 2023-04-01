using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repository.Context;
using Repository.Interfaces;
using System.Linq.Expressions;

namespace Repository.Implementations
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected CompEmpDbContext CompEmpDbContext;

        public RepositoryBase(CompEmpDbContext repositoryContext)
        {
            CompEmpDbContext = repositoryContext;
        }


        public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
        CompEmpDbContext.Set<T>()
        .AsNoTracking() :
        CompEmpDbContext.Set<T>();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,bool trackChanges) =>
       !trackChanges ?
        CompEmpDbContext.Set<T>()
        .Where(expression)
        .AsNoTracking() :
        CompEmpDbContext.Set<T>()
        .Where(expression);
        
        public IQueryable<T> GetAllAsync(bool tracking)
        {
            if (tracking)
            {
            return CompEmpDbContext.Set<T>();
            }
             return CompEmpDbContext.Set<T>().AsNoTracking();

        }

        public void Create(T entity) => CompEmpDbContext.Set<T>().Add(entity);
        public void Update(T entity) => CompEmpDbContext.Set<T>().Update(entity);
        public void Delete(T entity) => CompEmpDbContext.Set<T>().Remove(entity);
    }
}
