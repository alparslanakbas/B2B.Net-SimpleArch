using Business.Repositories.CustomerOperationClaimRepository;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOperationClaimsController : ControllerBase
    {
        private readonly ICustomerOperationClaimService _customerOperationClaimService;

        public CustomerOperationClaimsController(ICustomerOperationClaimService customerOperationClaimService)
        {
            _customerOperationClaimService = customerOperationClaimService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Add(CustomerOperationClaim customerOperationClaim)
        {
            var result = await _customerOperationClaimService.Add(customerOperationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(CustomerOperationClaim customerOperationClaim)
        {
            var result = await _customerOperationClaimService.Update(customerOperationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(CustomerOperationClaim customerOperationClaim)
        {
            var result = await _customerOperationClaimService.Delete(customerOperationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _customerOperationClaimService.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerOperationClaimService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetListDto()
        {
            var result = await _customerOperationClaimService.GetListDto();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
