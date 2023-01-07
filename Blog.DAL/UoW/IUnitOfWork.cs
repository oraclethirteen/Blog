using Blog.DAL.Repository;

namespace Blog.DAL.UoW
{
    /// <summary>
    /// Интерфейс хранилища всех репозиториев проекта
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges(bool ensureAutoHistory = false);

        //Метод сохранения всех изменений в БД (по всем репозиториям)
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
