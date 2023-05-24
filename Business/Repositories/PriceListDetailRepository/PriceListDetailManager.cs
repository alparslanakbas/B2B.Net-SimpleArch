using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.PriceListDetailRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.PriceListDetailRepository.Validation;
using Business.Repositories.PriceListDetailRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.PriceListDetailRepository;
using Entities.Dtos;
using Core.Utilities.Business;

namespace Business.Repositories.PriceListDetailRepository
{
    public class PriceListDetailManager : IPriceListDetailService
    {
        private readonly IPriceListDetailDal _priceListDetailDal;

        public PriceListDetailManager(IPriceListDetailDal priceListDetailDal)
        {
            _priceListDetailDal = priceListDetailDal;
        }


        // Fiyat Listesi Detay Ekle
        [SecuredAspect("Admin,Müşteri")]
        [ValidationAspect(typeof(PriceListDetailValidator))]
        [RemoveCacheAspect("IPriceListDetailService.Get")]
        public async Task<IResult> Add(PriceListDetail priceListDetail)
        {
            IResult result = BusinessRules.Run(await ChechIfProductExist(priceListDetail));
            if (result != null)
            {
                return result;
            }


            await _priceListDetailDal.Add(priceListDetail);
            return new SuccessResult(PriceListDetailMessages.Added);
        }
        //****************************************//

        // Fiyat Listesi Detay Güncelle
        [SecuredAspect("Admin,Müşteri")]
        [ValidationAspect(typeof(PriceListDetailValidator))]
        [RemoveCacheAspect("IPriceListDetailService.Get")]
        public async Task<IResult> Update(PriceListDetail priceListDetail)
        {
            await _priceListDetailDal.Update(priceListDetail);
            return new SuccessResult(PriceListDetailMessages.Updated);
        }
        //****************************************//

        // Fiyat Listesi Detay Sil
        [SecuredAspect("Admin,Müşteri")]
        [RemoveCacheAspect("IPriceListDetailService.Get")]
        public async Task<IResult> Delete(PriceListDetail priceListDetail)
        {
            await _priceListDetailDal.Delete(priceListDetail);
            return new SuccessResult(PriceListDetailMessages.Deleted);
        }
        //****************************************//

        // Fiyat Listelerinin Detaylarını Listele
        [SecuredAspect("Admin,Müşteri")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<PriceListDetail>>> GetList()
        {
            return new SuccessDataResult<List<PriceListDetail>>(await _priceListDetailDal.GetAll());
        }
        //****************************************//

        // Fiyat Listelerinin Detaylarını Id'ye Göre Listele
        [SecuredAspect("Admin,Müşteri")]
        public async Task<IDataResult<PriceListDetail>> GetById(int id)
        {
            return new SuccessDataResult<PriceListDetail>(await _priceListDetailDal.Get(p => p.Id == id));
        }
        //****************************************//


        // Fiyat Listelerini Ürün Id'ye Göre Getir
        [SecuredAspect("Admin,Müşteri")]
        public async Task<List<PriceListDetail>> GetListByProductId(int productId)
        {
            return await _priceListDetailDal.GetAll(x=>x.ProductId==productId);
        }
        //****************************************//


        // Fiyat Listelerine Bağlı Ürün Adlarını Getir
        [SecuredAspect("Admin,Müşteri")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<PriceListDetailDto>>> GetListProductName(int priceListId)
        {
            return new SuccessDataResult<List<PriceListDetailDto>>(await _priceListDetailDal.GetListProductName(priceListId));
        }
        //****************************************//

        private async Task<IResult> ChechIfProductExist(PriceListDetail priceListDetail)
        {
            var result = await _priceListDetailDal.Get(x=>x.PriceListId==priceListDetail.PriceListId && x.ProductId==priceListDetail.ProductId);

            if (result!= null)
            {
                return new ErrorResult("Bu Ürün Zaten Listeye Eklenmiş.!");
            }
            return new SuccessResult();
        }
    }
}
