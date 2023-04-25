using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Repositories.PriceListDetailRepository
{
    public interface IPriceListDetailService
    {
        Task<IResult> Add(PriceListDetail priceListDetail);
        Task<IResult> Update(PriceListDetail priceListDetail);
        Task<IResult> Delete(PriceListDetail priceListDetail);
        Task<IDataResult<List<PriceListDetail>>> GetList();
        Task<IDataResult<List<PriceListDetailDto>>> GetListProductName(int priceListId);
        Task<List<PriceListDetail>> GetListByProductId(int productId);
        Task<IDataResult<PriceListDetail>> GetById(int id);
    }
}
