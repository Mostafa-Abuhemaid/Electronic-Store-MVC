using My_Shop.Entities.Models;
using My_Shop.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.DataAccess.Implementation
{
    public class ShopingCardRepository : GenericRepository<ShopingCard>, IShopingCardRepositort
    {
        private readonly ApplicationDbContext _context;

        public ShopingCardRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public int Decrease(ShopingCard shopingCard, int count)
        {
            shopingCard.Count -= count;
            return shopingCard.Count;   
        }

        public int Increase(ShopingCard shopingCard, int count)
        {
            shopingCard.Count += count;
            return shopingCard.Count;
        }
    }
}
