using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using My_Shop.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.DataAccess.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<T> _tables;
        public GenericRepository(ApplicationDbContext dbContext)
        {
           _dbContext = dbContext;
            _tables = _dbContext.Set<T>();
        }
        public void add(T entity)
        {
            
            _tables.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? Perdicate = null, string? IncliudWord = null)
        {
            IQueryable<T> query = _tables;
            if (Perdicate != null)
            {
                query = query.Where(Perdicate);
            }
            if (IncliudWord != null)
            {
                foreach (var item in IncliudWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }
       


        public T GetFristOrDefault(System.Linq.Expressions.Expression<Func<T, bool>>? Perdicate = null, string? IncliudWord = null)
        {
            IQueryable<T> query = _tables;
            if (Perdicate != null)
            {
                query = query.Where(Perdicate);
            }
            if (IncliudWord != null)
            {
                foreach (var item in IncliudWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            _tables.Remove(entity);
        }

        public void RemoveInRange(IEnumerable<T> entities)
        {
            _tables.RemoveRange(entities);
        }
    }
}
