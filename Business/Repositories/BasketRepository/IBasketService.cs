using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Repositories.BasketRepository
{
    public interface IBasketService
    {
        Task<IResult> Add(Basket basket);
        Task<IResult> Update(Basket basket);
        Task<IResult> Delete(Basket basket);
        Task<IDataResult<List<Basket>>> GetList();
        Task<List<Basket>> GetListByProductId(int productId);
        Task<IDataResult<List<BasketListDto>>> GetListByCustomerId(int customerId);
        Task<IDataResult<Basket>> GetById(int id);
    }
}
