using My_Shop.Entities;
using My_Shop.Entities.Models;
using My_Shop.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.DataAccess.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepositort
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            _context = dbContext;
        }
        public void Update(Category category)
        {
            var categoryindb= _context.Categories.FirstOrDefault(x=>x.Id==category.Id);
            if (categoryindb!=null)
            {
              categoryindb.Id = category.Id;
              categoryindb.Name = category.Name;
              categoryindb.Description = category.Description;

            }
        }
    }
}
