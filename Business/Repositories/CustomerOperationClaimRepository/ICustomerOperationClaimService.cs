using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Repositories.CustomerOperationClaimRepository
{
    public interface ICustomerOperationClaimService
    {
        Task<IResult> Add(CustomerOperationClaim customerOperationClaim);
        Task<IResult> Update(CustomerOperationClaim customerOperationClaim);
        Task<IResult> Delete(CustomerOperationClaim customerOperationClaim);
        Task<IDataResult<List<CustomerOperationClaim>>> GetList();
        Task<IDataResult<CustomerOperationClaim>> GetById(int id);
        Task<IDataResult<List<CustomerOperationClaimListDto>>> GetListDto();
    }
}
