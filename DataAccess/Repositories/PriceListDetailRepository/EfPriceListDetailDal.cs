using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.PriceListDetailRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Core.Aspects.Caching;

namespace DataAccess.Repositories.PriceListDetailRepository
{
    public class EfPriceListDetailDal : EfEntityRepositoryBase<PriceListDetail, SimpleContextDb>, IPriceListDetailDal
    {
        
        public async Task<List<PriceListDetailDto>> GetListProductName(int priceListId)
        {
            using (var context = new SimpleContextDb())
            {
                var result = from priceListDetail in context.PriceListDetails.Where(x => x.PriceListId == priceListId)
                             join product in context.Products on priceListDetail.ProductId equals product.Id
                             select new PriceListDetailDto
                             {
                                 Id = priceListDetail.Id,
                                 ProductName = product.Name,
                                 ProductId = priceListDetail.ProductId,
                                 PriceListId = priceListDetail.PriceListId,
                                 Price = priceListDetail.Price
                             };
                return await result.OrderBy(x => x.ProductName).ToListAsync();
            }
        }
    }
}
