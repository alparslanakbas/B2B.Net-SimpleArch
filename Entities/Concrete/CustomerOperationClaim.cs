using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class CustomerOperationClaim
    {
        public int Id { get; set; }
        public int OperationClaimId { get; set; }
        public int CustomerId { get; set; }
    }
}
