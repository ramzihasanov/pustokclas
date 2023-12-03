namespace WebApplication6.Repositories
{
    public interface IBookRepository
    {
        Task CreateAsync(Book book);
        Task CreateBookTagAsync(BookTag booktag);
        Task CreateBookImageAsync(BookImage bookimage);
        void Delete(Book book);
        Task<Book> GetAsync(int id);
        Task<List<Book>> GetAllAsync();
        Task<List<Tag>> GetAllTagAsync();
        Task<List<Genre>> GetAllGenreAsync();
        Task<List<BookTag>> GetAllBookTagAsync();
        Task<List<Author>> GetAllAuthorAsync();
        Task<List<BookImage>> GetAllBookImagesAsync();
        Task<int> CommitAsync();

    }
}
