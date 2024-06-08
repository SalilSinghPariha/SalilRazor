using Salil.DataAccess.Repository.IRepository;
using Salil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public OrderHeaderRepository(ApplicationDBContext applicationDBContext):base(applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }

        public void Update(OrderHeader orderHeader)
        {
           _dbContext.Update(orderHeader);
        }

        public void UpdateStatus(int id, string status)
        {
            var objFromDb= _dbContext.orderHeaders.FirstOrDefault(x => x.Id == id);
            if (objFromDb != null)
            {
                objFromDb.OrderStatus = status;
            }
        }
    }
}
