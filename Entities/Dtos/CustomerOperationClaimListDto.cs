using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CustomerOperationClaimListDto:CustomerOperationClaim
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string OperationClaimName { get; set; }
    }
}
