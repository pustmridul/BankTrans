using BankTrans.Data;
using BankTrans.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankTrans.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransController : ControllerBase
    {
        private readonly ITransService _transService;
        public TransController(ITransService transService)
        {
            _transService = transService;
        }
        [HttpPost]
        public async Task<IActionResult> SaveTransData(CityBankTransaction model)
        {

            var result = _transService.SaveTransData(model);

            return Ok(result);
        }
    }
}
