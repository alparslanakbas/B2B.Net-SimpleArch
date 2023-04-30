using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Repositories.OrderDetailRepository
{
    public interface IOrderDetailDal : IEntityRepository<OrderDetail>
    {
        Task<List<OrderDetailListDto>> GetListOrderDetailsDto(int orderId);
    }
}
