using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Repositories.ProductRepository
{
    public interface IProductDal : IEntityRepository<Product>
    {
        Task<List<ProductListDto>>GetProductList(int customerId);
    }
}
