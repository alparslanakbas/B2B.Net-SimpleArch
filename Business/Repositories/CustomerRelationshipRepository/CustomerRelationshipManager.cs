using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.CustomerRelationshipRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.CustomerRelationshipRepository.Validation;
using Business.Repositories.CustomerRelationshipRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerRelationshipRepository;

namespace Business.Repositories.CustomerRelationshipRepository
{
    public class CustomerRelationshipManager : ICustomerRelationshipService
    {
        private readonly ICustomerRelationshipDal _customerRelationshipDal;

        public CustomerRelationshipManager(ICustomerRelationshipDal customerRelationshipDal)
        {
            _customerRelationshipDal = customerRelationshipDal;
        }


        // Müşteri Etkileşimi Ekle
        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        [ValidationAspect(typeof(CustomerRelationshipValidator))]
        [RemoveCacheAspect("ICustomerRelationshipService.Get")]
        public async Task<IResult> Add(CustomerRelationship customerRelationship)
        {
            await _customerRelationshipDal.Add(customerRelationship);
            return new SuccessResult(CustomerRelationshipMessages.Added);
        }
        //****************************************//

        // Müşteri Etkileşimi Güncelle
        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        [ValidationAspect(typeof(CustomerRelationshipValidator))]
        [RemoveCacheAspect("ICustomerRelationshipService.Get")]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Update(CustomerRelationship customerRelationship)
        {
            var result = await _customerRelationshipDal.Get(x=>x.CustomerId == customerRelationship.CustomerId);
            if (result != null)
            {
                customerRelationship.Id = result.Id;
                await _customerRelationshipDal.Update(customerRelationship);
            }
            else
            {
                await _customerRelationshipDal.Add(customerRelationship);
            }
            
            return new SuccessResult(CustomerRelationshipMessages.Updated);
        }
        //****************************************//

        // Müşteri Etkileşimi Sil
        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        [RemoveCacheAspect("ICustomerRelationshipService.Get")]
        public async Task<IResult> Delete(CustomerRelationship customerRelationship)
        {
            await _customerRelationshipDal.Delete(customerRelationship);
            return new SuccessResult(CustomerRelationshipMessages.Deleted);
        }
        //****************************************//

        // Müşteri Etkileşimlerini Listele
        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomerRelationship>>> GetList()
        {
            return new SuccessDataResult<List<CustomerRelationship>>(await _customerRelationshipDal.GetAll());
        }
        //****************************************//

        // Müşteri Etkileşimlerini Id'ye Göre Getir
        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        public async Task<IDataResult<CustomerRelationship>> GetById(int id)
        {
            return new SuccessDataResult<CustomerRelationship>(await _customerRelationshipDal.Get(p => p.Id == id));
        }


        [SecuredAspect("Admin,Müşteri,Kullanıcı")]
        public async Task<IDataResult<CustomerRelationship>> GetByCustomerId(int customerId)
        {
            return new SuccessDataResult<CustomerRelationship>(await _customerRelationshipDal.Get(p => p.CustomerId == customerId));
        }
        //****************************************//
    }
}
