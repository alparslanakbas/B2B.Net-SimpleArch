using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.PriceListRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.PriceListRepository.Validation;
using Business.Repositories.PriceListRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.PriceListRepository;

namespace Business.Repositories.PriceListRepository
{
    public class PriceListManager : IPriceListService
    {
        private readonly IPriceListDal _priceListDal;

        public PriceListManager(IPriceListDal priceListDal)
        {
            _priceListDal = priceListDal;
        }


        // Fiyat Listesi Ekle
        [SecuredAspect("Admin,Müşteri")]
        [ValidationAspect(typeof(PriceListValidator))]
        [RemoveCacheAspect("IPriceListService.Get")]
        public async Task<IResult> Add(PriceList priceList)
        {
            await _priceListDal.Add(priceList);
            return new SuccessResult(PriceListMessages.Added);
        }
        //****************************************//

        // Fiyat Listesi Güncelle
        [SecuredAspect("Admin,Müşteri")]
        [ValidationAspect(typeof(PriceListValidator))]
        [RemoveCacheAspect("IPriceListService.Get")]
        public async Task<IResult> Update(PriceList priceList)
        {
            await _priceListDal.Update(priceList);
            return new SuccessResult(PriceListMessages.Updated);
        }
        //****************************************//

        // Fiyat Listesi Sil
        [SecuredAspect("Admin,Müşteri")]
        [RemoveCacheAspect("IPriceListService.Get")]
        public async Task<IResult> Delete(PriceList priceList)
        {
            await _priceListDal.Delete(priceList);
            return new SuccessResult(PriceListMessages.Deleted);
        }
        //****************************************//

        // Fiyat Listelerini Listele
        [SecuredAspect("Admin,Müşteri")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<PriceList>>> GetList()
        {
            return new SuccessDataResult<List<PriceList>>(await _priceListDal.GetAll());
        }
        //****************************************//

        // Fiyat Listelerini Id'ye Göre Listele
        [SecuredAspect("Admin,Müşteri")]
        [CacheAspect()]
        public async Task<IDataResult<PriceList>> GetById(int id)
        {
            return new SuccessDataResult<PriceList>(await _priceListDal.Get(p => p.Id == id));
        }
        //****************************************//
    }
}
