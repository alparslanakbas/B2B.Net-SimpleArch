using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CustomerListDto:Customer
    {
        public int? PriceListId { get; set; }
        public string? PriceListName { get; set; }
        public decimal? Discount { get; set; }
    }
}
