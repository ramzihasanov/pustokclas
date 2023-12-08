using Pustok.Core.Models;
using WebApplication6.DAL;

namespace WebApplication6.ViewServices
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Setting>> GetBook()
        {
            var settings = _context.Settings.ToList();
            return settings;
        }
    }
}
