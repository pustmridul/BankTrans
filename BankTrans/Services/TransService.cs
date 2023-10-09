using BankTrans.Data;
using Serilog;

namespace BankTrans.Services
{
    public class TransService : ITransService
    {
        private readonly AppDbContext _context;
        public TransService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> SaveTransData(CityBankTransaction model)
        {
            var obj = new CityBankTransaction()
            {

            };

            obj = model;

            _context.Add(obj);
            try
            {
                Log.Information("Save Success");
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Fail:" + ex.Message);
                return false;
            }
        }
    }
}
