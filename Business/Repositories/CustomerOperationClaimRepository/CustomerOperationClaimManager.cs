using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.CustomerOperationClaimRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.CustomerOperationClaimRepository.Validation;
using Business.Repositories.CustomerOperationClaimRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerOperationClaimRepository;
using Entities.Dtos;

namespace Business.Repositories.CustomerOperationClaimRepository
{
    public class CustomerOperationClaimManager : ICustomerOperationClaimService
    {
        private readonly ICustomerOperationClaimDal _customerOperationClaimDal;

        public CustomerOperationClaimManager(ICustomerOperationClaimDal customerOperationClaimDal)
        {
            _customerOperationClaimDal = customerOperationClaimDal;
        }

        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(CustomerOperationClaimValidator))]
        [RemoveCacheAspect("ICustomerOperationClaimService.Get")]

        public async Task<IResult> Add(CustomerOperationClaim customerOperationClaim)
        {
            await _customerOperationClaimDal.Add(customerOperationClaim);
            return new SuccessResult(CustomerOperationClaimMessages.Added);
        }

        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(CustomerOperationClaimValidator))]
        [RemoveCacheAspect("ICustomerOperationClaimService.Get")]

        public async Task<IResult> Update(CustomerOperationClaim customerOperationClaim)
        {
            await _customerOperationClaimDal.Update(customerOperationClaim);
            return new SuccessResult(CustomerOperationClaimMessages.Updated);
        }

        [SecuredAspect("Admin")]
        [RemoveCacheAspect("ICustomerOperationClaimService.Get")]

        public async Task<IResult> Delete(CustomerOperationClaim customerOperationClaim)
        {
            await _customerOperationClaimDal.Delete(customerOperationClaim);
            return new SuccessResult(CustomerOperationClaimMessages.Deleted);
        }

        [SecuredAspect("Admin")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomerOperationClaim>>> GetList()
        {
            return new SuccessDataResult<List<CustomerOperationClaim>>(await _customerOperationClaimDal.GetAll());
        }

        [SecuredAspect("Admin")]
        public async Task<IDataResult<CustomerOperationClaim>> GetById(int id)
        {
            return new SuccessDataResult<CustomerOperationClaim>(await _customerOperationClaimDal.Get(p => p.Id == id));
        }

        [SecuredAspect("Admin")]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomerOperationClaimListDto>>> GetListDto()
        {
            return new SuccessDataResult<List<CustomerOperationClaimListDto>>(await _customerOperationClaimDal.GetListDto());
        }
    }
}
