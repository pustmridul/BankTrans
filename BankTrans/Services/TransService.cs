using BankTrans.Data;
using Microsoft.EntityFrameworkCore;

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
            
            var isAdded = await _context.SaveChangesAsync();
            return isAdded>0;
        }

        public async Task<ICollection<CityBankTransaction>> GetAll()
        {
            
            return await _context.CityBankTransactions.ToListAsync();
        }
    }
}
