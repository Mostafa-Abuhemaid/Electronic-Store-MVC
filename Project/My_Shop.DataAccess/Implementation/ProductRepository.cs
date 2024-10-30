using My_Shop.DataAccess;
using My_Shop.Entities.Models;
using My_Shop.Entities.ViewModels;
using My_Shop.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.DataAccess.Implementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepositort                                                                        
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            _context = dbContext;
        }
        public void Update(Product product)
        {
            var productindb= _context.Products.FirstOrDefault(x=>x.Id==product.Id);
            if (productindb!=null)
            {
              productindb.Id = product.Id;
              productindb.Name = product.Name;
              productindb.Description = product.Description;
                productindb.Price = product.Price;
                productindb.Img = product.Img;
                productindb.CategoryId = product.CategoryId;
               

            }
        }
    }
}
