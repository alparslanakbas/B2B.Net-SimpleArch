using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.CustomerRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.CustomerRepository.Validation;
using Business.Repositories.CustomerRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerRepository;

namespace Business.Repositories.CustomerRepository
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }


        // Müþteri Ekle
        [SecuredAspect()] 
        [ValidationAspect(typeof(CustomerValidator))]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Add(Customer customer)
        {
            await _customerDal.Add(customer);
            return new SuccessResult(CustomerMessages.Added);
        }
        //****************************************//

        // Müþteri Güncelle
        [SecuredAspect()]
        [ValidationAspect(typeof(CustomerValidator))]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Update(Customer customer)
        {
            await _customerDal.Update(customer);
            return new SuccessResult(CustomerMessages.Updated);
        }
        //****************************************//

        // Müþteri Sil
        [SecuredAspect()]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Delete(Customer customer)
        {
            await _customerDal.Delete(customer);
            return new SuccessResult(CustomerMessages.Deleted);
        }
        //****************************************//

        // Müþterileri Listele
        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Customer>>> GetList()
        {
            return new SuccessDataResult<List<Customer>>(await _customerDal.GetAll());
        }
        //****************************************//

        // Müþterileri Id'ye Göre Getir
        [SecuredAspect()]
        public async Task<IDataResult<Customer>> GetById(int id)
        {
            return new SuccessDataResult<Customer>(await _customerDal.Get(p => p.Id == id));
        }
        //****************************************//
    }
}
