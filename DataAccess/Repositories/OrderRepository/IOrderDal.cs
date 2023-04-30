using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Repositories.OrderRepository
{
    public interface IOrderDal : IEntityRepository<Order>
    {
        string getOrderNumber();
        Task<List<OrderListDto>> GetListOrderDto();
        Task<OrderListDto>GetByIdDto(int id);
    }
}
