using Microsoft.EntityFrameworkCore;

namespace WebApplication6.Repositories.IImplementations
{
    public class SliderRepository : ISliderRepository
    {
        private readonly AppDbContext _appDbContext;

        public SliderRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<int> CommitAsync()
        {
          return await _appDbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(Slider slider)
        {
           await _appDbContext.Slider.AddAsync(slider);
        }

        public void Delete(Slider slider)
        {
            _appDbContext.Remove(slider);
        }

        public async Task<List<Slider>> GetAllAsync()
        {
           return  await _appDbContext.Slider.ToListAsync();
                  
        }

        public async Task<Slider> GetSliderByIdAsync(int id)
        {
            return await _appDbContext.Slider.FirstOrDefaultAsync(x => x.Id == id);
        }

       
    }
 }

