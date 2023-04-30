using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.OrderRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.OrderRepository
{
    public class EfOrderDal : EfEntityRepositoryBase<Order, SimpleContextDb>, IOrderDal
    {
        public async Task<OrderListDto> GetByIdDto(int id)
        {
            using (var context = new SimpleContextDb())
            {
                var result = from order in context.Orders.Where(o => o.Id == id)
                             join customer in context.Customers on order.CustomerId equals customer.Id
                             select new OrderListDto
                             {
                                 Id = order.Id,
                                 CustomerId = order.CustomerId,
                                 CustomerName = customer.Name,
                                 Date = order.Date,
                                 OrderNumber = order.OrderNumber,
                                 Status = order.Status,
                                 Quantity = context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(x => x.Quantity),
                                 Total = context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(x => x.Price) *
                                 context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(x => x.Quantity)
                             };
                return await result.FirstOrDefaultAsync();
            }
        }

        public async Task<List<OrderListDto>> GetListOrderDto()
        {
            using (var context = new SimpleContextDb())
            {
                var result = from order in context.Orders
                             join customer in context.Customers on order.CustomerId equals customer.Id
                             select new OrderListDto
                             {
                                 Id = order.Id,
                                 CustomerId = order.CustomerId,
                                 CustomerName = customer.Name,
                                 Date = order.Date,
                                 OrderNumber = order.OrderNumber,
                                 Status = order.Status,
                                 Quantity = context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(x => x.Quantity),
                                 Total = context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(x => x.Price) *
                                 context.OrderDetails.Where(x => x.OrderId == order.Id).Sum(x => x.Quantity)
                             };
                return await result.OrderByDescending(x => x.Id).ToListAsync();
            }
        }

        public string getOrderNumber()
        {
            using (var context = new SimpleContextDb())
            {
               var findLastOrder=context.Orders.OrderByDescending(x=>x.Id).LastOrDefault();      
               if (findLastOrder==null)
               {
                    return "SP00000000000000";
               }
            string findLastOrderNumber = findLastOrder.OrderNumber;   
            findLastOrderNumber = findLastOrderNumber.Substring(2,14);
            int orderNumberInt =  Convert.ToInt32(findLastOrderNumber);   
            orderNumberInt++;
            string newOrderNumber = orderNumberInt.ToString();
            
            for (int i = newOrderNumber.Length; i < 14 ; i++)
            {
                newOrderNumber = "0"+ newOrderNumber;
            }
            newOrderNumber = "SP"+ newOrderNumber;
            return newOrderNumber;
            }
        }
    }
}
