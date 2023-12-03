namespace WebApplication6.Services
{
    public interface ISliderService
    {
        Task<Slider> GetAsync(int id);
        Task<List<Slider>>GetAllSAsync();
        Task CreateAsync(Slider slider);
        Task DeleteAsync(int id);
        Task UpdateAsync(Slider slider);
    }
}
