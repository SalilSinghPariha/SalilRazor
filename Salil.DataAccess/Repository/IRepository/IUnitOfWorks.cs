using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository.IRepository
{
    public interface IUnitOfWorks : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IFoodTypeRepository foodTypeRepository { get; }       
        IMenuItemRepository menuItemRepository { get; }

        IShoppingCartRepository shoppingCartRepository { get; }

        IOrderHeaderRepository orderHeaderRepository { get; }
        IOrderDetailsRepository orderDetailsRepository { get; }
        IApplicationUserRepository applicationUserRepository { get; }
        void Save();

    }
}
