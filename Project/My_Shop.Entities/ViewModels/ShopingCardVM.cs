using My_Shop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.Entities.ViewModels
{
    public class ShopingCardVM
    {
        public IEnumerable<ShopingCard> CartsList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public Decimal? TotalCard {  get; set; }
    }
}
