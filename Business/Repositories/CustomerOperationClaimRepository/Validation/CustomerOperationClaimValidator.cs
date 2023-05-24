using System;
using System.Collections.Generic;
using FluentValidation;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Business.Repositories.CustomerOperationClaimRepository.Validation
{
    public class CustomerOperationClaimValidator : AbstractValidator<CustomerOperationClaim>
    {
        public CustomerOperationClaimValidator()
        {
        }
    }
}
