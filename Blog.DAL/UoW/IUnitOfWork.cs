using Blog.DAL.Repository;

namespace Blog.DAL.UoW
{
    // Интерфейс хранилища всех репозиториев проекта
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges(bool ensureAutoHistory = false);

        // Метод сохранения всех изменений в БД (по всем репозиториям)
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
