using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Repositories.CustomerRepository
{
    public interface ICustomerService
    {
        Task<IResult> Add(CustomerRegisterDto request);
        Task<IResult> Update(Customer request);
        Task<IResult> Delete(Customer request);
        Task<IDataResult<List<CustomerListDto>>> GetList();
        Task<IDataResult<Customer>> GetById(int id);
        Task<IDataResult<CustomerListDto>> GetByCustomerDto(int id);
        Task<Customer> GetByEmail(string email);
    }
}
