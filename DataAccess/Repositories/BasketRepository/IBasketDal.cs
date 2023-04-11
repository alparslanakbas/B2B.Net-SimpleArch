using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Repositories.BasketRepository
{
    public interface IBasketDal : IEntityRepository<Basket>
    {
        Task <List<BasketListDto>> GetListByCustomerId(int customerId);
    }
}
