using WebApplication6.Models;
using WebApplication6.Services.Interfaces;

namespace WebApplication6.Services.IImplementations
{
    public class AuthorService : IAuthorService
    {
        public Task CreateAsync(Author entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Author>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
