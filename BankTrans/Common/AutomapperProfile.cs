using AutoMapper;
using BankTrans.Data;
using BankTrans.Models.Dtos;

namespace BankTrans.Common
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        {
            CreateMap<CityBankTransactionCreateDto, CityBankTransaction>();
        }
    }
}
