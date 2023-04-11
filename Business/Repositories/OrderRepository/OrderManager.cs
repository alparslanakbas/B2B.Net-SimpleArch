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
using Entities.Dtos;
using Business.Repositories.OrderDetailRepository;
using Business.Repositories.BasketRepository;

namespace Business.Repositories.OrderRepository
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IBasketService _basketService;

        public OrderManager(IOrderDal orderDal, IOrderDetailService orderDetailService, IBasketService basketService)
        {
            _orderDal = orderDal;
            _orderDetailService = orderDetailService;
            _basketService = basketService;
        }


        // Sipariş Ekle
        // [SecuredAspect()]
        [ValidationAspect(typeof(OrderValidator))]
        [RemoveCacheAspect("IOrderService.Get")]
        public async Task<IResult> Add(int customerId)
        {
            var baskets = await _basketService.GetListByCustomerId(customerId);
            string newOrderNumber = _orderDal.getOrderNumber();
            Order order = new Order(){
                Id=0,
                CustomerId=baskets.Data[0].CustomerId,
                Date = DateTime.Now,
                OrderNumber= newOrderNumber,
                Status = "Onay Bekliyor"
            };
            
            await _orderDal.Add(order);
            foreach (var basket in baskets.Data)
            {
                OrderDetail orderDetail = new ()
                {
                    Id=0,
                    OrderId = order.Id,
                    Price= basket.Price,
                    ProductId = basket.ProductId,
                    Quantity = basket.Quantity
                };
                await _orderDetailService.Add(orderDetail);
                Basket basketEntity = new(){
                    Id=basket.Id,
                    CustomerId= basket.CustomerId,
                    Price= basket.Price,
                    ProductId = basket.ProductId,
                    Quantity = basket.Quantity
                };
                await _basketService.Delete(basketEntity);
            }
            return new SuccessResult(OrderMessages.Added);
        }
        //****************************************//

        // Sipariş Güncelle
        //[SecuredAspect()]
        [ValidationAspect(typeof(OrderValidator))]
        [RemoveCacheAspect("IOrderService.Get")]
        public async Task<IResult> Update(Order order)
        {
            await _orderDal.Update(order);
            return new SuccessResult(OrderMessages.Updated);
        }
        //****************************************//

        // Sipariş Sil
        //[SecuredAspect()]
        [RemoveCacheAspect("IOrderService.Get")]
        public async Task<IResult> Delete(Order order)
        {
            var details = await _orderDetailService.GetList(order.Id);
            foreach (var detail in details.Data)
            {
                await _orderDetailService.Delete(detail);
            }
            await _orderDal.Delete(order);
            return new SuccessResult(OrderMessages.Deleted);
        }
        //****************************************//

        // Siparişleri Listele
        //[SecuredAspect()]
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
