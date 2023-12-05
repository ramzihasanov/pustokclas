using WebApplication6.Repositories.Interfaces;

namespace WebApplication6.Repositories.IImplementations
{
    public class BookTagRepository : GenericRepository<BookTag>, IBookTagRepository
    {
        public BookTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
