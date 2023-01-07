namespace Blog.DAL.Repository
{
    /// <summary>
    /// Интерфейс репозитория, описывающий CRUD-операции моделей
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<int> Create(T item);
        IEnumerable<T> GetAll();
        Task<T> Get(int id);
        Task<int> Update(T item);
        Task<int> Delete(T item);
    }
}
