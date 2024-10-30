using My_Shop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.Entities.ViewModels
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public  IEnumerable<OrderDetail> orderDetails { get; set; }

    }
}
