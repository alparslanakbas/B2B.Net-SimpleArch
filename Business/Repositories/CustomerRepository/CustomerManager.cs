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
using Entities.Dtos;
using Core.Utilities.Hashing;
using Core.Utilities.Business;

namespace Business.Repositories.CustomerRepository
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;

        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }


        // Müşteri Ekle
        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(CustomerValidator))]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Add(CustomerRegisterDto request)
        {
             IResult result = BusinessRules.Run(
                await CheckIfEmailExists(request.Email)
                );

            if (result != null)
            {
                return result;
            }

            byte[] passwordHash,passwordSalt;
            HashingHelper.CreatePassword(request.Password,out passwordHash,out passwordSalt);
            Customer customer = new Customer{
                Id=0,
                Name=request.Name,
                Email=request.Email,
                PasswordHash=passwordHash,
                PasswordSalt=passwordSalt
            };
            await _customerDal.Add(customer);
            return new SuccessResult(CustomerMessages.Added);
        }
        //****************************************//

        // Müşteri Güncelle
        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(CustomerValidator))]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Update(Customer request)
        {
            await _customerDal.Update(request);
            return new SuccessResult(CustomerMessages.Updated);
        }
        //****************************************//

        // Müşteri Sil
        [SecuredAspect("Admin")]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Delete(Customer request)
        {
            await _customerDal.Delete(request);
            return new SuccessResult(CustomerMessages.Deleted);
        }
        //****************************************//

        // Müşterileri Listele
        [SecuredAspect("Admin")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Customer>>> GetList()
        {
            return new SuccessDataResult<List<Customer>>(await _customerDal.GetAll());
        }
        //****************************************//

        // Müşterileri Id'ye Göre Getir
        [SecuredAspect("Admin")]
        public async Task<IDataResult<Customer>> GetById(int id)
        {
            return new SuccessDataResult<Customer>(await _customerDal.Get(p => p.Id == id));
        }
        //****************************************//

        // Müşterileri Mail Adresine Göre Getir
        public async Task<Customer> GetByEmail(string email)
        {
            var result = await _customerDal.Get(x=>x.Email==email);
            return result;
        }
        //****************************************//


        // Aynı Mail Adresini Kontrol Etme İşlemi
        private async Task<IResult> CheckIfEmailExists(string email)
        {
            var list = await GetByEmail(email);
            if (list != null)
            {
                return new ErrorResult("Bu Mail Adresi Sistem de Zaten Mevcut.!");
            }
            return new SuccessResult();
        }

        //****************************************//

    }
}
