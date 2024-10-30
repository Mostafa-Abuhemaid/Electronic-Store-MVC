using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.Entities.Repository
{
    public interface IGenericRepository<T> where T : class
    {
       IEnumerable<T> GetAll(Expression<Func<T,bool>>?Perdicate=null,string?IncliudWord = null);

        T GetFristOrDefault(Expression<Func<T, bool>> ?Perdicate = null, string? IncliudWord = null);
        void add(T entity); 
        void Remove(T entity);
        void RemoveInRange(IEnumerable<T> entities);


    }
}
