using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.CustomerRepository;
using DataAccess.Context.EntityFramework;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.CustomerRepository
{
    public class EfCustomerDal : EfEntityRepositoryBase<Customer, SimpleContextDb>, ICustomerDal
    {
        public async Task<CustomerListDto> GetDto(int id)
        {
            using (var context = new SimpleContextDb())
            {
                var result = from customer in context.Customers.Where(p => p.Id == id)
                             select new CustomerListDto
                             {
                                 Id = customer.Id,
                                 Email = customer.Email,
                                 Name = customer.Name,
                                 PasswordHash = customer.PasswordHash,
                                 PasswordSalt = customer.PasswordSalt,
                                 Discount =
                                 (context.CustomerRelationships.Where(p => p.CustomerId == customer.Id) != null
                                 ? context.CustomerRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.Discount).FirstOrDefault()
                                : 0),
                                 PriceListId =
                                 (context.CustomerRelationships.Where(p => p.CustomerId == customer.Id) != null
                                ? context.CustomerRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault()
                                : 0),
                                 PriceListName =
                                 (context.CustomerRelationships.Where(p => p.CustomerId == customer.Id) != null
                                 ? context.PriceLists.Where(p => p.Id == (context.CustomerRelationships.Where(p => p.CustomerId == customer.Id).Select(s => s.PriceListId).FirstOrDefault())).Select(s => s.Name).FirstOrDefault()
                                 : "")
                             };
                return await result.FirstOrDefaultAsync();
            }
        }

        public async Task<List<CustomerListDto>> GetListDto()
        {
            using (var context = new SimpleContextDb())
            {
                var result = from customer in context.Customers
                             select new CustomerListDto
                             {
                                 Id = customer.Id,
                                 Email = customer.Email,
                                 Name = customer.Name,
                                 PasswordHash = customer.PasswordHash,
                                 PasswordSalt = customer.PasswordSalt,
                                 Discount= (context.CustomerRelationships.Where(x => x.CustomerId == customer.Id) != null)
                                 ? context.CustomerRelationships.Where(x => x.CustomerId == customer.Id).Select(x => x.Discount).FirstOrDefault() : 0,
                                 PriceListId =(context.CustomerRelationships.Where(x=>x.CustomerId == customer.Id)!=null)
                                 ? context.CustomerRelationships.Where(x=>x.CustomerId==customer.Id).Select(x=>x.PriceListId).FirstOrDefault() : 0,
                                 PriceListName = (context.CustomerRelationships.Where(x=>x.CustomerId==customer.Id)!=null
                                 ? context.PriceLists.Where(x=>x.Id==(context.CustomerRelationships.Where(x=>x.CustomerId==customer.Id).Select(x=>x.PriceListId).FirstOrDefault()))
                                 .Select(x=>x.Name).FirstOrDefault():"")
                             };
                return await result.OrderBy(x => x.Name).ToListAsync();
            }
        }
    }
}
