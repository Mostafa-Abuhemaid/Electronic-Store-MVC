using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.Entities.Repository
{
    public interface IUniteOfWork:IDisposable
    {
        ICategoryRepositort category { get; }
        IProductRepositort product { get; }
        IShopingCardRepositort shopingcard { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        IApplicationUserRepository ApplicationUser { get; }

        int Compelet();
    }
}
