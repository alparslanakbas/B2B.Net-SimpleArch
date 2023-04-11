using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.ProductRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.ProductRepository
{
    public class EfProductDal : EfEntityRepositoryBase<Product, SimpleContextDb>, IProductDal
    {
        public async Task<List<ProductListDto>> GetProductList(int customerId)
        {
            using(var context = new SimpleContextDb())
            {
                var customerRelationShip= context.CustomerRelationships.Where(x=>x.CustomerId==customerId).SingleOrDefault();
                var result = from product in context.Products
                select new ProductListDto{
                    Id=product.Id,
                    Name=product.Name,
                    Discount= customerRelationShip.Discount,
                    Price = context.PriceListDetails.Where(x=>x.PriceListId==customerRelationShip.PriceListId && x.ProductId == product.Id).Count()>0 ?
                    context.PriceListDetails.Where(x=>x.PriceListId==customerRelationShip.PriceListId && x.ProductId == product.Id).Select(y=>y.Price).SingleOrDefault():0,
                    MainImageUrl = (context.ProductImages.Where(x=>x.ProductId==product.Id && x.MainImage==true).Count()>0
                    ? context.ProductImages.Where(x=>x.ProductId==product.Id && x.MainImage==true).Select(x=>x.ImageUrl).FirstOrDefault()
                    :""),
                    Images=context.ProductImages.Where(x=>x.ProductId==product.Id).Select(x=>x.ImageUrl).ToList()
                       
                };
                return await result.OrderBy(x=>x.Name).ToListAsync();
            }
        }
    }
}
