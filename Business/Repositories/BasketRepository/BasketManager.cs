using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.BasketRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.BasketRepository.Validation;
using Business.Repositories.BasketRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.BasketRepository;
using Entities.Dtos;

namespace Business.Repositories.BasketRepository
{
    public class BasketManager : IBasketService
    {
        private readonly IBasketDal _basketDal;

        public BasketManager(IBasketDal basketDal)
        {
            _basketDal = basketDal;
        }

        // Sepete Ürün Ekle
        // [SecuredAspect()]
        [ValidationAspect(typeof(BasketValidator))]
        [RemoveCacheAspect("IBasketService.Get")]
        public async Task<IResult> Add(Basket basket)
        {
            await _basketDal.Add(basket);
            return new SuccessResult(BasketMessages.Added);
        }
        //****************************************//

        // Sepetteki Ürünleri Güncelle
        //[SecuredAspect()]
        [ValidationAspect(typeof(BasketValidator))]
        [RemoveCacheAspect("IBasketService.Get")]
        public async Task<IResult> Update(Basket basket)
        {
            await _basketDal.Update(basket);
            return new SuccessResult(BasketMessages.Updated);
        }
        //****************************************//

        // Sepetteki Ürünleri Sil
        //[SecuredAspect()]
        [RemoveCacheAspect("IBasketService.Get")]
        public async Task<IResult> Delete(Basket basket)
        {
            await _basketDal.Delete(basket);
            return new SuccessResult(BasketMessages.Deleted);
        }
        //****************************************//

        // Sepetteki Ürünleri Listele
        // [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Basket>>> GetList()
        {
            return new SuccessDataResult<List<Basket>>(await _basketDal.GetAll());
        }
        //****************************************//

        // Sepetteki Ürünleri Id'ye Göre Getir
        [SecuredAspect()]
        public async Task<IDataResult<Basket>> GetById(int id)
        {
            return new SuccessDataResult<Basket>(await _basketDal.Get(p => p.Id == id));
        }
        //****************************************//


        // Sipariş Ürünlerini Müşteri Id'ye Özel Listele
        public async Task<IDataResult<List<BasketListDto>>> GetListByCustomerId(int customerId)
        {
            return new SuccessDataResult<List<BasketListDto>>(await _basketDal.GetListByCustomerId(customerId));
        }

        public async Task<List<Basket>> GetListByProductId(int productId)
        {
            return await _basketDal.GetAll(x=>x.ProductId==productId);
        }
        //****************************************//

    }
}
