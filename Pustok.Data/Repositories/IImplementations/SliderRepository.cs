using Microsoft.EntityFrameworkCore;
using WebApplication6.Repositories.Interfaces;

namespace WebApplication6.Repositories.IImplementations
{
    public class SliderRepository : GenericRepository<Slider>,ISliderRepository
    {
        private readonly AppDbContext _context;

        public SliderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
        {
          return await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Slider slider)
        {
           await _context.Slider.AddAsync(slider);
        }

        public void Delete(Slider slider)
        {
            _context.Remove(slider);
        }

        public async Task<List<Slider>> GetAllAsync()
        {
           return  await _context.Slider.ToListAsync();
                  
        }

        public async Task<Slider> GetSliderByIdAsync(int id)
        {
            return await _context.Slider.FirstOrDefaultAsync(x => x.Id == id);
        }

       
    }
 }

