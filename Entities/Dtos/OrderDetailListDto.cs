using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class OrderDetailListDto : OrderDetail
    {
        public string ProductName { get; set; }
        public decimal Total { get; set; }
    }
}
