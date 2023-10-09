using BankTrans.Data;

namespace BankTrans.Services
{
    public interface ITransService
    {
        Task<bool> SaveTransData(CityBankTransaction model);
    }
}
