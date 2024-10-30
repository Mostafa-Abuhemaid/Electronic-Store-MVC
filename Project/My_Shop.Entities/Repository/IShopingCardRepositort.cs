using My_Shop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.Entities.Repository
{
    public interface IShopingCardRepositort :IGenericRepository<ShopingCard>
    {
        public int Increase(ShopingCard shopingCard,int count);
        public int Decrease(ShopingCard shopingCard, int count);    

    }
}
