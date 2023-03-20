using Contracts;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected CompEmpDbContext CompEmpDbContext;
        public RepositoryBase(CompEmpDbContext repositoryContext)
        => CompEmpDbContext = repositoryContext;

        public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
        CompEmpDbContext.Set<T>()
        .AsNoTracking() :
        CompEmpDbContext.Set<T>();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges) =>
        !trackChanges ?
        CompEmpDbContext.Set<T>()
        .Where(expression)
        .AsNoTracking() :
        CompEmpDbContext.Set<T>()
        .Where(expression);
        public void Create(T entity) => CompEmpDbContext.Set<T>().Add(entity);
        public void Update(T entity) => CompEmpDbContext.Set<T>().Update(entity);
        public void Delete(T entity) => CompEmpDbContext.Set<T>().Remove(entity);
    }
}
