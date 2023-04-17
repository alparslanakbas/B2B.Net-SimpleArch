using Business.Aspects.Secured;
using Business.Repositories.EmailParameterRepository.Constans;
using Business.Repositories.EmailParameterRepository.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Validation;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.EmailParameterRepository;
using Entities.Concrete;
using System.Net;
using System.Net.Mail;

namespace Business.Repositories.EmailParameterRepository
{
    public class EmailParameterManager : IEmailParameterService
    {
        private readonly IEmailParameterDal _emailParameterDal;

        public EmailParameterManager(IEmailParameterDal emailParameterDal)
        {
            _emailParameterDal = emailParameterDal;
        }


        // Mail Parametresi Ekle
        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(EmailParameterValidator))]
        [RemoveCacheAspect("IEmailParameterService.Get")]
        public async Task<IResult> Add(EmailParameter emailParameter)
        {
            await _emailParameterDal.Add(emailParameter);
            return new SuccessResult(EmailParameterMessages.AddedEmailParameter);

        }
        //****************************************//

        // Mail Parametresini Güncelle
        [SecuredAspect("Admin")]
        [ValidationAspect(typeof(EmailParameterValidator))]
        [RemoveCacheAspect("IEmailParameterService.Get")]
        public async Task<IResult> Update(EmailParameter emailParameter)
        {
            await _emailParameterDal.Update(emailParameter);
            return new SuccessResult(EmailParameterMessages.UpdatedEmailParameter);
        }
        //****************************************//

        // Mail Parametresi Sil
        [SecuredAspect("Admin")]
        [RemoveCacheAspect("IEmailParameterService.Get")]
        public async Task<IResult> Delete(EmailParameter emailParameter)
        {
            await _emailParameterDal.Delete(emailParameter);
            return new SuccessResult(EmailParameterMessages.DeletedEmailParameter);
        }
        //****************************************//

        // Mail Parametresi Getir Id'ye Göre
        [SecuredAspect("Admin")]
        public async Task<IDataResult<EmailParameter>> GetById(int id)
        {
            return new SuccessDataResult<EmailParameter>(await _emailParameterDal.Get(p => p.Id == id));
        }
        //****************************************//

        // Mail Parametresi İlk Kaydı Getir
        public async Task<EmailParameter> GetFirst()
        {
            return await _emailParameterDal.GetFirst();
        }
        //****************************************//

        // Mail Parametlerini Listele
        [SecuredAspect("Admin")]
        [CacheAspect()]
        public async Task<IDataResult<List<EmailParameter>>> GetList()
        {
            return new SuccessDataResult<List<EmailParameter>>(await _emailParameterDal.GetAll());
        }
        //****************************************//

        // Mail Gönder
        [SecuredAspect("Admin")]
        public async Task<IResult> SendEmail(EmailParameter emailParameter, string body, string subject, string emails)
        {
            using (MailMessage mail = new MailMessage())
            {
                string[] setEmails = emails.Split(",");
                mail.From = new MailAddress(emailParameter.Email);
                foreach (var email in setEmails)
                {
                    mail.To.Add(email);
                }
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = emailParameter.Html;
                //mail.Attachments.Add();
                using (SmtpClient smtp = new SmtpClient(emailParameter.Smtp))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailParameter.Email, emailParameter.Password);
                    smtp.EnableSsl = emailParameter.SSL;
                    smtp.Port = emailParameter.Port;
                    await smtp.SendMailAsync(mail);
                }
            }
            return new SuccessResult(EmailParameterMessages.EmailSendSuccesiful);

        }
        //****************************************//


    }
}
