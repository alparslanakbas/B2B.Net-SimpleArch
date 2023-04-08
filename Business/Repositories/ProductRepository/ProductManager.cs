using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.ProductRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.ProductRepository.Validation;
using Business.Repositories.ProductRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductRepository;

namespace Business.Repositories.ProductRepository
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }


        // Ürün Ekle
        [SecuredAspect("Admin,Product.add")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Add(Product product)
        {
            await _productDal.Add(product);
            return new SuccessResult(ProductMessages.Added);
        }
        //****************************************//

        // Ürün Güncelle
        [SecuredAspect("Admin,Product.update")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Update(Product product)
        {
            await _productDal.Update(product);
            return new SuccessResult(ProductMessages.Updated);
        }
        //****************************************//

        // Ürün Sil
        [SecuredAspect("Admin,Product.delete")]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Delete(Product product)
        {
            await _productDal.Delete(product);
            return new SuccessResult(ProductMessages.Deleted);
        }
        //****************************************//

        // Ürünleri Listele
        [SecuredAspect("Admin,Product.get")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Product>>> GetList()
        {
            return new SuccessDataResult<List<Product>>(await _productDal.GetAll());
        }
        //****************************************//

        // Ürünleri Id'ye Göre Listele
        [SecuredAspect("Admin,Product.get")]
        public async Task<IDataResult<Product>> GetById(int id)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(p => p.Id == id));
        }
        //****************************************//
    }
}
