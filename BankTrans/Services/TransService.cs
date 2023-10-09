using BankTrans.Data;

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
            await   _context.SaveChangesAsync();
            return await _context.SaveChangesAsync()>0 ? true : false;

        }
    }
}
