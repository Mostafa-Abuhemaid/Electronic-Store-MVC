using My_Shop.Entities.Models;
using My_Shop.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.DataAccess.Implementation
{
	public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
	{
		private readonly ApplicationDbContext _context;
		public OrderDetailRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(OrderDetail Orderdetail)
		{
			_context.orderDetails.Update(Orderdetail);
		}
	}
}
