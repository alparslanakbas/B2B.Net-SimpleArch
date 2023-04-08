using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.OrderRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.OrderRepository.Validation;
using Business.Repositories.OrderRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.OrderRepository;

namespace Business.Repositories.OrderRepository
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;

        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        
        // Sipariş Ekle
        [SecuredAspect()]
        [ValidationAspect(typeof(OrderValidator))]
        [RemoveCacheAspect("IOrderService.Get")]
        public async Task<IResult> Add(Order order)
        {
            await _orderDal.Add(order);
            return new SuccessResult(OrderMessages.Added);
        }
        //****************************************//

        // Sipariş Güncelle
        [SecuredAspect()]
        [ValidationAspect(typeof(OrderValidator))]
        [RemoveCacheAspect("IOrderService.Get")]
        public async Task<IResult> Update(Order order)
        {
            await _orderDal.Update(order);
            return new SuccessResult(OrderMessages.Updated);
        }
        //****************************************//

        // Sipariş Sil
        [SecuredAspect()]
        [RemoveCacheAspect("IOrderService.Get")]
        public async Task<IResult> Delete(Order order)
        {
            await _orderDal.Delete(order);
            return new SuccessResult(OrderMessages.Deleted);
        }
        //****************************************//

        // Siparişleri Listele
        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Order>>> GetList()
        {
            return new SuccessDataResult<List<Order>>(await _orderDal.GetAll());
        }
        //****************************************//

        // Siparişleri Id'ye Göre Listele
        [SecuredAspect()]
        public async Task<IDataResult<Order>> GetById(int id)
        {
            return new SuccessDataResult<Order>(await _orderDal.Get(p => p.Id == id));
        }
        //****************************************//
    }
}
