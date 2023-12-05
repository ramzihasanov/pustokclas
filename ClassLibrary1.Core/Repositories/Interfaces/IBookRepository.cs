namespace WebApplication6.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        
        Task CreateBookTagAsync(BookTag booktag);
        Task CreateBookImageAsync(BookImage bookimage);     
        Task<List<Book>> GetAllAsync();
        Task<List<Tag>> GetAllTagAsync();
        Task<List<Genre>> GetAllGenreAsync();
        Task<List<BookTag>> GetAllBookTagAsync();
        Task<List<Author>> GetAllAuthorAsync();
        Task<List<BookImage>> GetAllBookImagesAsync();
       

    }
}
