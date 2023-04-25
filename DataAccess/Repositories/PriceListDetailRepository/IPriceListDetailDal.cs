using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Repositories.PriceListDetailRepository
{
    public interface IPriceListDetailDal : IEntityRepository<PriceListDetail>
    {
        Task<List<PriceListDetailDto>> GetListProductName(int priceListId);
    }
}
