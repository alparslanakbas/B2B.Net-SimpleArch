using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Repositories.OrderRepository
{
    public interface IOrderService
    {
        Task<IResult> Add(int customerId);
        Task<IResult> Update(Order order);
        Task<IResult> Delete(Order order);
        Task<IDataResult<List<Order>>> GetList();
        Task<IDataResult<List<OrderListDto>>> GetListWithOrderDto();
        Task<IDataResult<List<Order>>> GetListByCustomerId(int customerId);
        Task<IDataResult<Order>> GetById(int id);
        Task<IDataResult<OrderListDto>> GetByIdDto(int id);
    }
}
