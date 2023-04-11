using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.BasketRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.BasketRepository
{
    public class EfBasketDal : EfEntityRepositoryBase<Basket, SimpleContextDb>, IBasketDal
    {
        public async Task<List<BasketListDto>> GetListByCustomerId(int customerId)
        {
            using (var context = new SimpleContextDb())
            {
                var result = from basket in context.Baskets.Where(x=>x.CustomerId==customerId)
                             join product in context.Products on basket.ProductId equals product.Id
                             select new BasketListDto{
                                Id=basket.Id,
                                CustomerId = basket.CustomerId,
                                ProductId = basket.ProductId,
                                ProductName = product.Name,
                                Price = basket.Price,
                                Quantity = basket.Quantity,
                                TotalPrice = basket.Price * basket.Quantity
                             };
                return await result.ToListAsync();             
            }
        }
    }
}
