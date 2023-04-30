using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.OrderDetailRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.OrderDetailRepository.Validation;
using Business.Repositories.OrderDetailRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.OrderDetailRepository;
using Entities.Dtos;
using System.Security.Cryptography.X509Certificates;

namespace Business.Repositories.OrderDetailRepository
{
    public class OrderDetailManager : IOrderDetailService
    {
        private readonly IOrderDetailDal _orderDetailDal;

        public OrderDetailManager(IOrderDetailDal orderDetailDal)
        {
            _orderDetailDal = orderDetailDal;
        }


        // Sipariş Bilgisi Ekle
        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(OrderDetailValidator))]
        [RemoveCacheAspect("IOrderDetailService.Get")]
        public async Task<IResult> Add(OrderDetail orderDetail)
        {
            await _orderDetailDal.Add(orderDetail);
            return new SuccessResult(OrderDetailMessages.Added);
        }
        //****************************************//

        // Sipariş Bilgisi Güncelle
        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(OrderDetailValidator))]
        [RemoveCacheAspect("IOrderDetailService.Get")]

        public async Task<IResult> Update(OrderDetail orderDetail)
        {
            await _orderDetailDal.Update(orderDetail);
            return new SuccessResult(OrderDetailMessages.Updated);
        }
        //****************************************//

        // Sipariş Bilgisi Sil
        [SecuredAspect("Admin")]
        [RemoveCacheAspect("IOrderDetailService.Get")]

        public async Task<IResult> Delete(OrderDetail orderDetail)
        {
            await _orderDetailDal.Delete(orderDetail);
            return new SuccessResult(OrderDetailMessages.Deleted);
        }
        //****************************************//

        // Sipariş Bilgilerini Listele
        [SecuredAspect("Admin")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<OrderDetail>>> GetList(int orderId)
        {
            return new SuccessDataResult<List<OrderDetail>>(await _orderDetailDal.GetAll(x=>x.OrderId==orderId));
        }
        //****************************************//

        // Sipariş Bilgilerini Id'ye Göre Getir
        [SecuredAspect("Admin")]
        public async Task<IDataResult<OrderDetail>> GetById(int id)
        {
            return new SuccessDataResult<OrderDetail>(await _orderDetailDal.Get(p => p.Id == id));
        }
        //****************************************//


        // İlgili Ürüne Ait Tüm Sipariş Detaylarını Getir
        [SecuredAspect("Admin")]
        public async Task<List<OrderDetail>> GetListByProductId(int productId)
        {
            return await _orderDetailDal.GetAll(x=>x.ProductId==productId);
        }

        // Sipariş Detaylarına Ürün Adını Getirme
        [SecuredAspect("Admin")]
        public async Task<IDataResult<List<OrderDetailListDto>>> GetListProductOrderDetail(int orderId)
        {
            return new SuccessDataResult<List<OrderDetailListDto>>(await _orderDetailDal.GetListOrderDetailsDto(orderId));
        }
        //****************************************//
    }
}
