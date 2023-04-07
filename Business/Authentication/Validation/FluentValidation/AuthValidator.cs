using Entities.Dtos;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class AuthValidator : AbstractValidator<RegisterAuthDto>
    {
        public AuthValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Kullanıcı Adı Boş Olamaz.!");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Mail Adresi Boş Olamaz.!");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Geçerli Bir Mail Adresi Yazın.!");
            RuleFor(p => p.Image.FileName).NotEmpty().WithMessage("Kullanıcı Resmi Boş Olamaz.!");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Şifre Boş Olamaz.!");
            RuleFor(p => p.Password).MinimumLength(6).WithMessage("Şifre En Az 6 Karakter Olmalıdır.!");
            RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifreniz En Az 1 Adet Büyük Harf İçermelidir.!");
            RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifreniz En Az 1 Adet Küçük Harf İçermelidir.!");
            RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Şifreniz En Az 1 Adet Sayı İçermelidir.!");
            RuleFor(p => p.Password).Matches("[^a-zA-Z0-9]").WithMessage("Şifreniz En Az 1 Adet Özel Karakter İçermelidir.!");
        }
    }
}
