using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.CustomerOperationClaimRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.CustomerOperationClaimRepository
{
    public class EfCustomerOperationClaimDal : EfEntityRepositoryBase<CustomerOperationClaim, SimpleContextDb>, ICustomerOperationClaimDal
    {
        public async Task<List<CustomerOperationClaimListDto>> GetListDto()
        {
            using (var context = new SimpleContextDb())
            {
                var result = from customer in context.Customers
                             join customerOperationClaim in context.CustomerOperationClaims on customer.Id equals customerOperationClaim.CustomerId into customerClaims
                             from customerClaim in customerClaims.DefaultIfEmpty()
                             join operationClaim in context.OperationClaims on customerClaim.OperationClaimId equals operationClaim.Id into claims
                             from claim in claims.DefaultIfEmpty()
                             select new CustomerOperationClaimListDto()
                             {
                                 Id = customerClaim != null ? customerClaim.Id : 0,
                                 CustomerId = customer.Id,
                                 CustomerName = customer.Name,
                                 CustomerEmail = customer.Email,
                                 OperationClaimId = claim != null ? claim.Id : 0,
                                 OperationClaimName = claim != null ? claim.Name : ""
                             };
                return await result.OrderByDescending(x => x.Id).ToListAsync();
            }
        }


    }
}
