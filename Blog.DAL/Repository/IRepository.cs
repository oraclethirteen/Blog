using System.Linq.Expressions;

namespace Blog.DAL.Repository
{
    /// <summary>
    /// Интерфейс репозитория, описывающий CRUD-операции моделей
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<int> Create(TEntity item);
        IEnumerable<TEntity> GetAll();
        Task<TEntity> Get(int id);

        public Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties);

        Task<int> Update(TEntity item);
        Task<int> Delete(TEntity item);
    }
}
