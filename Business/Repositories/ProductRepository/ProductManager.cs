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
using Entities.Dtos;
using Business.Repositories.ProductImageRepository;
using Business.Repositories.PriceListDetailRepository;
using Core.Utilities.Business;
using Business.Repositories.BasketRepository;
using Business.Repositories.OrderDetailRepository;

namespace Business.Repositories.ProductRepository
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IProductImageService _productImageService;
        private readonly IPriceListDetailService _priceListDetailService;
        private readonly IBasketService _basketService;
        private readonly IOrderDetailService _orderDetailService;

        public ProductManager(IProductDal productDal, IProductImageService productImageService, IPriceListDetailService priceListDetailService, IBasketService basketService, IOrderDetailService orderDetailService)
        {
            _productDal = productDal;
            _productImageService = productImageService;
            _priceListDetailService = priceListDetailService;
            _basketService = basketService;
            _orderDetailService = orderDetailService;
        }


        // Ürün Ekle
        [SecuredAspect("Admin,Müşteri")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Add(Product product)
        {
            await _productDal.Add(product);
            return new SuccessResult(ProductMessages.Added);
        }
        //****************************************//

        // Ürün Güncelle
        [SecuredAspect("Admin,Müşteri")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Update(Product product)
        {
            await _productDal.Update(product);
            return new SuccessResult(ProductMessages.Updated);
        }
        //****************************************//

        // Ürün Sil
        [SecuredAspect("Admin,Müşteri")]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Delete(Product product)
        {
            IResult result = BusinessRules.Run(
            await CheckIfProductExistToBaskets(product.Id),
            await CheckIfProductExistToOrderDetails(product.Id)
            );

            if (result !=null)
            {
                return result;
            }

            var images = await _productImageService.GetListByProductId(product.Id);
            foreach (var image in images.Data)
            {
                await _productImageService.Delete(image);
            }

            var priceListProducts = await _priceListDetailService.GetListByProductId(product.Id);
            foreach (var item in priceListProducts)
            {
                await _priceListDetailService.Delete(item);
            }

            await _productDal.Delete(product);
            return new SuccessResult(ProductMessages.Deleted);
        }
        //****************************************//

        // Ürünleri Listele
        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductListDto>>> GetList()
        {
            return new SuccessDataResult<List<ProductListDto>>(await _productDal.GetList());
        }
        //****************************************//

        // Ürünleri Id'ye Göre Listele
        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        public async Task<IDataResult<Product>> GetById(int id)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(p => p.Id == id));
        }

        //****************************************//

        // Ürün Listesini Müşteriye Göre Getir
        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductListDto>>> GetProductList(int customerId)
        {
            return new SuccessDataResult<List<ProductListDto>>(await _productDal.GetProductList(customerId));
        }

        //****************************************//


        // Ürün Silerken Sepette Olup Olmadığının Kontrolü Eğer Sepette Varsa Silinmez
        [SecuredAspect("Admin,Müşteri")]
        public async Task <IResult>  CheckIfProductExistToBaskets(int productId)
        {
            var result = await _basketService.GetListByProductId(productId);
            if (result.Count()>0)
            {
                return new ErrorResult("Silmeye Çalıştığınız Ürün Sepette Bulunuyor.!");    
            }
            return new SuccessResult();
        }
        //****************************************//


        // Ürün Silerken Siparişte Olup Olmadığının Kontrolü Eğer Siparişte Varsa Silinmez
        [SecuredAspect("Admin,Müşteri")]
        public async Task <IResult>  CheckIfProductExistToOrderDetails(int productId)
        {
            var result = await _orderDetailService.GetListByProductId(productId);
            if (result.Count() > 0)
            {
                return new ErrorResult("Silmeye Çalıştığınız Ürünün Siparişi Bulunuyor.!");    
            }
            return new SuccessResult();
        }
        //****************************************//
    }
}
