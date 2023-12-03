namespace WebApplication6.Repositories
{
    public interface ISliderRepository
    {
        Task CreateAsync(Slider slider);
        Task<List<Slider>> GetAllAsync();
        Task<Slider> GetSliderByIdAsync(int id);
        
        void Delete(Slider slider);
        Task<int> CommitAsync();
      
    }
}
