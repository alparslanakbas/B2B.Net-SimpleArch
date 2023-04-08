using Entities.Concrete;
using FluentValidation;

namespace Business.Repositories.EmailParameterRepository.Validation
{
    internal class EmailParameterValidator : AbstractValidator<EmailParameter>
    {
        public EmailParameterValidator()
        {
            RuleFor(p => p.Smtp).NotEmpty().WithMessage("SMTP Adresi Boş Olamaz.!");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Mail Adresi Boş Olamaz.!");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre Adresi Boş Olamaz.!");
            RuleFor(p => p.Port).NotEmpty().WithMessage("Port Adresi Boş Olamaz.!");
        }
    }
}
