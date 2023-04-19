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
using Entities.Dtos;
using Business.Abstract;
using Core.Utilities.Business;
using Core.Aspects.Transaction;

namespace Business.Repositories.ProductImageRepository
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IProductImageDal _productImageDal;
        private readonly IFileService _fileService;

        public ProductImageManager(IProductImageDal productImageDal, IFileService fileService)
        {
            _productImageDal = productImageDal;
            _fileService = fileService;
        }


        // Ürün Resmi Ekle
        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]
        public async Task<IResult> Add(ProductImageAddDto request)
        {
            foreach (var image in request.Image)
            {
                IResult result = BusinessRules.Run(
                   CheckIfImageExtesionsAllow(image.FileName),
                   CheckIfImageSizeIsLessThanOneMb(image.Length)
                   );

                if (result == null)
                {
                    string fileName = _fileService.FileSaveToServer(image, "C:/Users/legol/Desktop/b2b-front-end/src/assets/images/");
                    ProductImage productImage = new()
                    {
                        Id = 0,
                        ImageUrl = fileName,
                        ProductId = request.ProductId,
                        MainImage = false
                    };
                    await _productImageDal.Add(productImage);
                }
            }
            return new SuccessResult(ProductImageMessages.Added);
        }
        //****************************************//

        // Ürün Resmi Güncelle
        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]
        public async Task<IResult> Update(ProductImageUpdateDto request)
        {
            IResult result = BusinessRules.Run(
                CheckIfImageExtesionsAllow(request.Image.FileName),
                CheckIfImageSizeIsLessThanOneMb(request.Image.Length)
                );

            if (result != null)
            {
                return result;
            }

            string path = @"./Content/img/" + request.ImageUrl;
            _fileService.FileDeleteToServer(path);
            string fileName = _fileService.FileSaveToServer(request.Image, "./Content/img/");

            ProductImage productImage = new()
            {
                Id = request.Id,
                ProductId = request.ProductId,
                ImageUrl = fileName,
                MainImage = request.MainImage
            };

            await _productImageDal.Update(productImage);
            return new SuccessResult(ProductImageMessages.Updated);
        }
        //****************************************//

        // Ürün Resmi Sil
        [SecuredAspect("Admin")]
        [RemoveCacheAspect("IProductImageService.Get")]
        public async Task<IResult> Delete(ProductImage request)
        {
            string path = @"./Content/img/" + request.ImageUrl;
            _fileService.FileDeleteToServer(path);
            await _productImageDal.Delete(request);
            return new SuccessResult(ProductImageMessages.Deleted);
        }
        //****************************************//

        // Ürün Resimlerini Listele
        [SecuredAspect("Admin")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetList()
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll());
        }
        //****************************************//

        // Ürün Resimlerini Id'ye Göre Listele
        [SecuredAspect("Admin")]
        [CacheAspect()]
        public async Task<IDataResult<ProductImage>> GetById(int id)
        {
            return new SuccessDataResult<ProductImage>(await _productImageDal.Get(p => p.Id == id));
        }
        //****************************************//


        // Ürün Resimlerinin Boyutlarını Kontrol Et
        private IResult CheckIfImageSizeIsLessThanOneMb(long imgSize)
        {
            decimal imgMbSize = Convert.ToDecimal(imgSize * 0.000001);
            if (imgMbSize > 10)
            {
                return new ErrorResult("Yüklediğiniz Resmin Boyutu 10MB Küçük Olmalıdır.!");
            }
            return new SuccessResult();
        }
        //****************************************//

        // Ürün Resimlerinin Formatlarını Kontrol Et
        private IResult CheckIfImageExtesionsAllow(string fileName)
        {
            var ext = fileName.Substring(fileName.LastIndexOf('.'));
            var extension = ext.ToLower();
            List<string> AllowFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png" };
            if (!AllowFileExtensions.Contains(extension))
            {
                return new ErrorResult("Eklediğiniz Resim .jpg, .jpeg, .gif, .png Türlerinden Biri Olmalıdır.!");
            }
            return new SuccessResult();
        }
        //****************************************//

        // Ürünün Genel Olarak Tek Resimle Yansıtılması Yani 1 Ürünün Ana Resmini Alma
        [SecuredAspect("Admin")]
        [TransactionAspect()]
        public async Task<IResult> SetMainImage(int id)
        {
            var productImage = await _productImageDal.Get(x=>x.Id==id);
            var productImages = await _productImageDal.GetAll(x=>x.ProductId==productImage.ProductId);
            foreach (var x in productImages)
            {
                x.MainImage= false;
                await _productImageDal.Update(x);
            }
            productImage.MainImage=true;
            await _productImageDal.Update(productImage);
            return new SuccessResult(ProductImageMessages.MainImageUpdate);
        }
        //****************************************//

        // Ürüne Göre Listeleme İşlemi
        [SecuredAspect("Admin")]
        [CacheAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetListByProductId(int productId)
        {
            return new SuccessDataResult<List<ProductImage>> (await _productImageDal.GetAll(x=>x.ProductId==productId));
        }
        //****************************************//
    }
}
