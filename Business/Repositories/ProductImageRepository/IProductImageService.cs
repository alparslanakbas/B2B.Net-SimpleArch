using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;

namespace Business.Repositories.ProductImageRepository
{
    public interface IProductImageService
    {
        Task<IResult> Add(ProductImage productImage);
        Task<IResult> Update(ProductImage productImage);
        Task<IResult> Delete(ProductImage productImage);
        Task<IDataResult<List<ProductImage>>> GetList();
        Task<IDataResult<ProductImage>> GetById(int id);
    }
}
