using Microsoft.EntityFrameworkCore;
using ShopApp.Models;
using ShopApp.Repositories.Interfaces;

namespace ShopApp.Repositories
{
    public class Repositorty<T> : IRepository<T> where T : class
    {
        protected readonly ShopAppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repositorty(ShopAppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Get()
        {
            return _dbSet.AsQueryable(); //nee la san pham moi laf _context.sanphams.......
        }

        public IQueryable<T> FromSqlRaw(string sql,params object[] parameters)
        {
            return _dbSet.FromSqlRaw(sql, parameters);
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<bool> Exits(int id)
        {
            return await _dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        
    }
}
