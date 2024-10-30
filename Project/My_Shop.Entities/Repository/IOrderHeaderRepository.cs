﻿using My_Shop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.Entities.Repository
{
	public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
	{
		void Update(OrderHeader orderHeader);
		void UpdateStatus(int id, string? OrderStatus, string? PaymentStatus);

	}
}