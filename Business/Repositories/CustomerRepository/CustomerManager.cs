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
using Business.Repositories.CustomerRelationshipRepository;
using Business.Repositories.OrderRepository;

namespace Business.Repositories.CustomerRepository
{
    public class CustomerManager : ICustomerService
    {
        private readonly ICustomerDal _customerDal;
        private readonly ICustomerRelationshipService _customerRelationshipService;
        private readonly IOrderService _orderService;

        public CustomerManager(ICustomerDal customerDal, ICustomerRelationshipService customerRelationshipService, IOrderService orderService)
        {
            _customerDal = customerDal;
            _customerRelationshipService = customerRelationshipService;
            _orderService = orderService;
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
            IResult result = BusinessRules.Run(
                await CheckIfEmailExists(request.Email));

            if (result != null) { return result; }

            await _customerDal.Update(request);
            return new SuccessResult(CustomerMessages.Updated);
        }
        //****************************************//

        // Müşteri Sil
        [SecuredAspect("Admin")]
        [RemoveCacheAspect("ICustomerService.Get")]
        public async Task<IResult> Delete(Customer request)
        {
            IResult result = BusinessRules.Run(
            await CheckIfCustomerOrderExist(request.Id));

            if (result != null) { return result; }
                

            var customerRelationShip = await _customerRelationshipService.GetByCustomerId(request.Id);
            if (customerRelationShip.Data!=null)
            {
                await _customerRelationshipService.Delete(customerRelationShip.Data);
            }

            await _customerDal.Delete(request);
            return new SuccessResult(CustomerMessages.Deleted);
        }
        //****************************************//

        // Müşterileri Listele
        [SecuredAspect("Admin")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<CustomerListDto>>> GetList()
        {
            return new SuccessDataResult<List<CustomerListDto>>(await _customerDal.GetListDto());
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
            var result = await _customerDal.GetAll(x => x.Email == email);
            return result.FirstOrDefault();
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

        // Müşterinin Sipariş Olup Olmadığını Kontrol Eder
        public async Task<IResult> CheckIfCustomerOrderExist(int customerId)
        {
            var result = await _orderService.GetListByCustomerId(customerId);
            if (result.Data.Count>0)
            {
                return new ErrorResult("Siparişi Bulunan Müşteri Kaydı Silinemez.!");
            }
            return new SuccessResult();
        }


        // Müşterileri Dto daki Proplara Göre Getir
        [SecuredAspect("Admin")]
        public async Task<IDataResult<CustomerListDto>> GetByCustomerDto(int id)
        {
            return new SuccessDataResult<CustomerListDto>(await _customerDal.GetDto(id));
        }
    }
}
