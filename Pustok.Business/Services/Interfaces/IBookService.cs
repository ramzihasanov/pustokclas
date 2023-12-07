using WebApplication6.Models;

namespace WebApplication6.Services.Interfaces
{
    public interface IBookService
    {
        Task CreateAsync(Book book);
        Task DeleteAsync(int id);
        Task<List<Book>> GetAllAsync();
      
        Task<Book> GetAsync(int id);
        Task UpdateAsync(Book book);
    }
}
