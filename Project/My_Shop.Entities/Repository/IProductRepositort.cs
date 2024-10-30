using My_Shop.Entities.Models;
using My_Shop.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.Entities.Repository
{
    public interface IProductRepositort : IGenericRepository<Product>
    {
       // void add(ProductVM productVM);
        void Update(Product product);
    }
}
