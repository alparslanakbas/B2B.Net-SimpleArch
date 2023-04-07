using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;

namespace Business.Repositories.CustomerRepository
{
    public interface ICustomerService
    {
        Task<IResult> Add(Customer customer);
        Task<IResult> Update(Customer customer);
        Task<IResult> Delete(Customer customer);
        Task<IDataResult<List<Customer>>> GetList();
        Task<IDataResult<Customer>> GetById(int id);
    }
}
