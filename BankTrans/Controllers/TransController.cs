using AutoMapper;
using BankTrans.Data;
using BankTrans.Models.Dtos;
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
        private readonly IMapper _mapper;
        public TransController(ITransService transService, ITokenService tokenService,IMapper mapper)
        {
            _transService = transService;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost]

        public async Task<IActionResult> SaveTransData(CityBankTransactionCreateDto model)
        {
            CityBankTransaction cityBankTransaction = new CityBankTransaction();
            _mapper.Map(model, cityBankTransaction);
            cityBankTransaction.CreatedDate = DateTime.Now;
            var result = await _transService.SaveTransData(cityBankTransaction);

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
