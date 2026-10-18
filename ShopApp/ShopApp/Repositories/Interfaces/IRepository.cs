namespace ShopApp.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get();
        IQueryable<T> FromSqlRaw(string sql, params object[] parameters);
        Task<T?> GetByIdAsync(int id);
        Task<bool> Exits(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync();     


    }
}
