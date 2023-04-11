using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Repositories.ProductImageRepository
{
    public interface IProductImageService
    {
        Task<IResult> Add(ProductImageAddDto request);
        Task<IResult> Update(ProductImageUpdateDto request);
        Task<IResult> Delete(ProductImage productImage);
        Task<IDataResult<List<ProductImage>>> GetList();
        Task<List<ProductImage>> GetListByProductId(int productId);
        Task<IDataResult<ProductImage>> GetById(int id);
        Task<IResult> SetMainImage(int id);

    }
}
