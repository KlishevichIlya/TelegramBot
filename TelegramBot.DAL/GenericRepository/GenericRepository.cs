using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TelegramBot.DAL.Data;

namespace TelegramBot.DAL.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;

        public GenericRepository(ApplicationContext context) => _context = context;

        void IGenericRepository<T>.Add(T entity) => _context.Set<T>().Add(entity);

        void IGenericRepository<T>.AddRange(IEnumerable<T> entities) => _context.Set<T>().AddRange(entities);

        IEnumerable<T> IGenericRepository<T>.Find(Expression<Func<T, bool>> expression) => _context.Set<T>().Where(expression);

        IEnumerable<T> IGenericRepository<T>.GetAll() => _context.Set<T>().ToList();

        T IGenericRepository<T>.GetById(int id) => _context.Set<T>().Find(id);

        void IGenericRepository<T>.Remove(T entity) => _context.Set<T>().Remove(entity);

        void IGenericRepository<T>.Removerange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);
    }
}
