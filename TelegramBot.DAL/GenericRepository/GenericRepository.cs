using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TelegramBot.DAL.Data;

namespace TelegramBot.DAL.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        async Task IGenericRepository<T>.AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        async Task IGenericRepository<T>.AddRangeAsync(IEnumerable<T> entities) => await _context.Set<T>().AddRangeAsync(entities);

        async Task<IEnumerable<T>> IGenericRepository<T>.FindAsync(Expression<Func<T, bool>> expression) => await _context.Set<T>().Where(expression).ToListAsync();

        async Task<IEnumerable<T>> IGenericRepository<T>.GetAllAsync() => await _context.Set<T>().ToListAsync();

        async Task<T> IGenericRepository<T>.GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        void IGenericRepository<T>.Remove(T entity) => _context.Set<T>().Remove(entity);

        void IGenericRepository<T>.RemoveRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);
    }
}
