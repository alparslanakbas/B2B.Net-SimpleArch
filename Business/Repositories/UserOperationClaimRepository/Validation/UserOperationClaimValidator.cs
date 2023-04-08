using Entities.Concrete;
using FluentValidation;

namespace Business.Repositories.UserOperationClaimRepository.Validation
{
    public class UserOperationClaimValidator : AbstractValidator<UserOperationClaim>
    {
        public UserOperationClaimValidator()
        {
            RuleFor(p => p.UserId).Must(IsIdValid).WithMessage("Yetki Atamak için Kullanıcı Seçimi Yapmalısınız.!");
            RuleFor(p => p.OperationClaimId).Must(IsIdValid).WithMessage("Yetki Ataması İçin Yetki Seçimi Yapmalısınız.!");
        }

        private bool IsIdValid(int id)
        {
            if (id > 0 && id != null)
            {
                return true;
            }
            return false;
        }
    }
}
