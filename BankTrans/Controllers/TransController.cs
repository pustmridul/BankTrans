using BankTrans.Data;
using BankTrans.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BankTrans.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransController : ControllerBase
    {
        private readonly ITransService _transService;
        private readonly ITokenService _tokenService;
        public TransController(ITransService transService, ITokenService tokenService)
        {
            _transService = transService;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveTransData(CityBankTransaction model)
        {
            var result = await _transService.SaveTransData(model);

            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _transService.GetAll();

            return Ok(result);
        }

        [HttpGet("GetToken")]
        public async Task<IActionResult> GetToken()
        {
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "Mediasoft"),
                new Claim(ClaimTypes.Email, "mediasoft@bd.com"),
                new Claim("uid", "0909"),
                new Claim("user_name", "Test"),
              
            };
           var r= _tokenService.GenerateAccessToken(claims);
           return Ok(r);
        }


     
    }
}
