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
    public class UniteOfWork : IUniteOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepositort category { get; private set; }
        public IProductRepositort product { get; private set; }
        public IShopingCardRepositort shopingcard { get; private set; }
		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IOrderDetailRepository OrderDetail { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

		public UniteOfWork(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            category= new CategoryRepository(dbContext);
            product= new ProductRepository(dbContext);
            shopingcard= new ShopingCardRepository(dbContext);
			OrderHeader = new OrderHeaderRepository(dbContext);
            OrderDetail = new OrderDetailRepository(dbContext);
            ApplicationUser = new ApplicationUserRepository(dbContext);

        }
        public int Compelet()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
