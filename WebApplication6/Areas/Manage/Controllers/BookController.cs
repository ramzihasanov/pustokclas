using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication6.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext Context)
        {
            _context = Context;
        }
        public IActionResult Index()
        {

            var book = _context.Books.ToList();
            return View(book);
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            if (!ModelState.IsValid) return View(book);
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found!!!");
                return View();
            }
            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found!!!");
                return View();
            }
            var check = false;
            if (book.TagIds != null)
            {
                foreach (var tagId in book.TagIds)
                {
                    if (!_context.Tags.Any(x => x.Id == tagId))
                        check = true;
                }
            }
            if (check)
            {
                ModelState.AddModelError("TagId", "Tag not found!");
                return View(book);
            }
            else
            {
                if (book.TagIds != null)
                {
                    foreach (var tagId in book.TagIds)
                    {
                        BookTag bookTag = new BookTag
                        {
                            Book = book,
                            TagId = tagId
                        };
                        _context.BookTags.Add(bookTag);
                    }
                }
            }

            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid) return View();
            var existBook = _context.Books.Include(x => x.BookTags).FirstOrDefault(x => x.Id == id);

            existBook.TagIds = existBook.BookTags.Select(x => x.TagId).ToList();
            return View(existBook);
        }

        [HttpPost]
        public IActionResult Update(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            var existBook = _context.Books.Include(x => x.BookTags).FirstOrDefault(x => x.Id == book.Id);
            if (existBook == null) return NotFound();
            if (!ModelState.IsValid) return View(book);
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found!!!");
                return View();
            }
            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found!!!");
                return View();
            }
            existBook.BookTags.RemoveAll(x => !book.TagIds.Contains(x.TagId));
            foreach (var item in book.TagIds.Where(x => !existBook.BookTags.Any(y => y.TagId == x)))
            {
                BookTag booktag = new BookTag
                {
                    TagId = item
                };  
                existBook.BookTags.Add(booktag);
            }

            existBook.Name = book.Name;
            existBook.Description = book.Description;
            existBook.CostPrice = book.CostPrice;
            existBook.DisPrice = book.DisPrice;
            existBook.Code = book.Code;
            existBook.SalePrice = book.SalePrice;
            existBook.Tax = book.Tax;
            existBook.IsAvailable = book.IsAvailable;
            existBook.AuthorId = book.AuthorId;
            existBook.GenreId = book.GenreId;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var wanted = _context.Books.FirstOrDefault(x => x.Id == id);
            if (wanted == null) return NotFound();
            return View(wanted);
        }
        [HttpPost]
        public IActionResult Delete(Book book)
        {
            var wanted = _context.Books.FirstOrDefault(x => x.Id == book.Id);
            if (wanted == null) return NotFound();
            _context.Books.Remove(wanted);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}