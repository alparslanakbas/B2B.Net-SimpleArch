using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Resource;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Repositories.CustomerOperationClaimRepository
{
    public interface ICustomerOperationClaimDal : IEntityRepository<CustomerOperationClaim>
    {
        Task<List<CustomerOperationClaimListDto>> GetListDto();
    }
}
