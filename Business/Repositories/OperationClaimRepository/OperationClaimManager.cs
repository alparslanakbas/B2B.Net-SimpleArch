using Business.Aspects.Secured;
using Business.Repositories.OperationClaimRepository.Constans;
using Business.Repositories.OperationClaimRepository.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.OperationClaimRepository;
using Entities.Concrete;

namespace Business.Repositories.OperationClaimRepository
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;
        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        // Yetki Ekleme İşlemi
        //[SecuredAspect()] // Web Tarafı Yapıldıktan Sonra Açılacak, Şuanlık Elle Yeni İzinler Ekleneceği İçin Açık.!
        [ValidationAspect(typeof(OperationClaimValidator))]
        [RemoveCacheAspect("IOperationClaimService.Get")]
        public async Task<IResult> Add(OperationClaim operationClaim)
        {
            IResult result = BusinessRules.Run(await IsNameExistForAdd(operationClaim.Name));
            if (result != null)
            {
                return result;
            }

            await _operationClaimDal.Add(operationClaim);
            return new SuccessResult(OperationClaimMessages.Added);
        }
        //****************************************//

        // Yetkiyi Güncelleme İşlemi
        [SecuredAspect()]
        [ValidationAspect(typeof(OperationClaimValidator))]
        [RemoveCacheAspect("IOperationClaimService.Get")]
        public async Task<IResult> Update(OperationClaim operationClaim)
        {
            IResult result = BusinessRules.Run(await IsNameExistForUpdate(operationClaim));
            if (result != null)
            {
                return result;
            }

            await _operationClaimDal.Update(operationClaim);
            return new SuccessResult(OperationClaimMessages.Updated);
        }
        //****************************************//

        // Yetki Silme İşlemi
        [SecuredAspect()]
        [RemoveCacheAspect("IOperationClaimService.Get")]
        public async Task<IResult> Delete(OperationClaim operationClaim)
        {
            await _operationClaimDal.Delete(operationClaim);
            return new SuccessResult(OperationClaimMessages.Deleted);
        }
        //****************************************//

        // Yetkileri Listeleme İşlemi
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<OperationClaim>>> GetList()
        {
            return new SuccessDataResult<List<OperationClaim>>(await _operationClaimDal.GetAll());
        }
        //****************************************//

        // Yetkileri Id'ye Göre Getir
        public async Task<IDataResult<OperationClaim>> GetById(int id)
        {
            var result = await _operationClaimDal.Get(p => p.Id == id);
            return new SuccessDataResult<OperationClaim>(result);
        }
        //****************************************//

        // Id' ye Göre Yetkinin Bilgilerini Getir
        public async Task<OperationClaim> GetByIdForUserService(int id)
        {
            var result = await _operationClaimDal.Get(p => p.Id == id);
            return result;
        }
        //****************************************//

        // Eklenilecek Yetkinin Datada Zaten Var Olup Olmadığını Kontorl Eder
        private async Task<IResult> IsNameExistForAdd(string name)
        {
            var result = await _operationClaimDal.Get(p => p.Name == name);
            if (result != null)
            {
                return new ErrorResult(OperationClaimMessages.NameIsNotAvaible);
            }
            return new SuccessResult();
        }
        //****************************************//

        // Güncellenicek Yetkinin Datada Adının Zaten Var Olup Olmadığını Kontorl Eder
        private async Task<IResult> IsNameExistForUpdate(OperationClaim operationClaim)
        {
            var currentOperationClaim = await _operationClaimDal.Get(p => p.Id == operationClaim.Id);
            if (currentOperationClaim.Name != operationClaim.Name)
            {
                var result = await _operationClaimDal.Get(p => p.Name == operationClaim.Name);
                if (result != null)
                {
                    return new ErrorResult(OperationClaimMessages.NameIsNotAvaible);
                }
            }
            return new SuccessResult();
        }
        //****************************************//
    }
}
