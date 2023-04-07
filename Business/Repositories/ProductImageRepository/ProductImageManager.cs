using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.ProductImageRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.ProductImageRepository.Validation;
using Business.Repositories.ProductImageRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductImageRepository;

namespace Business.Repositories.ProductImageRepository
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IProductImageDal _productImageDal;

        public ProductImageManager(IProductImageDal productImageDal)
        {
            _productImageDal = productImageDal;
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Add(ProductImage productImage)
        {
            await _productImageDal.Add(productImage);
            return new SuccessResult(ProductImageMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Update(ProductImage productImage)
        {
            await _productImageDal.Update(productImage);
            return new SuccessResult(ProductImageMessages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Delete(ProductImage productImage)
        {
            await _productImageDal.Delete(productImage);
            return new SuccessResult(ProductImageMessages.Deleted);
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetList()
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll());
        }

        [SecuredAspect()]
        public async Task<IDataResult<ProductImage>> GetById(int id)
        {
            return new SuccessDataResult<ProductImage>(await _productImageDal.Get(p => p.Id == id));
        }

    }
}
