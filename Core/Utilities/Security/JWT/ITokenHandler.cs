using Entities.Concrete;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHandler
    {
        UserToken CreateUserToken(User user, List<OperationClaim> operationClaims);
        CustomerToken CreateCustomerUserToken(Customer customer, List<OperationClaim> operationClaims);
    }
}
