using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Helpers;

namespace WebApplication6.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(AppDbContext Context,IWebHostEnvironment env)
        {
            _context = Context;
            _env = env;
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
                    if (!_context.Tags.Any(x => x.Id == tagId)) { 
                    check = true;
                    break;
                     }
                }
            }
            if (check)
            {
                ModelState.AddModelError("TagId", "Tag not found!!!");
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
            if (book.FaceImage != null)
            {

                if (book.FaceImage.ContentType != "image/png" && book.FaceImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("FaceImage", "ancaq sekil yukle");
                    return View();
                }

                if (book.FaceImage.Length > 1048576)
                {
                    ModelState.AddModelError("FaceImage", "1 mb dan az yukle pul yazir ");
                    return View();
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", book.FaceImage);
                BookImage bookImage = new BookImage
                {
                    book = book,
                    ImageUrl = newFileName,
                    IsPoster = true,
                };
                _context.BookImages.Add(bookImage);
            };


            if (book.BackViewImg != null)
            {

                if (book.BackViewImg.ContentType != "image/png" && book.BackViewImg.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("BackViewImg", "ancaq sekil yuklemek olur");
                    return View();
                }

                if (book.BackViewImg.Length > 1048576)
                {
                    ModelState.AddModelError("BackViewImg", "1 mb dan cox yukleme ");
                    return View();
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", book.BackViewImg);
                BookImage bookImage = new BookImage
                {
                    book = book,
                    ImageUrl = newFileName,
                    IsPoster = false,
                };
                _context.BookImages.Add(bookImage);
            };

            if (book.BookDetailImgs != null)
            {
                foreach (var img in book.BookDetailImgs)
                {
                    string fileName = img.FileName;
                    if (img.ContentType != "image/png" && img.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("BookDetailImgs", "ancaq sekilyukle");
                        return View();
                    }

                    if (img.Length > 1048576)
                    {
                        ModelState.AddModelError("BookDetailImgs", "1 mb dan cox yukleme ");
                        return View();
                    }

                    string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", img);
                    BookImage bookImage = new BookImage
                    {
                        book = book,
                        ImageUrl = newFileName,
                        IsPoster = null,
                    };
                    _context.BookImages.Add(bookImage);
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
            existBook.BookImages.RemoveAll(x => !book.BookImagesIds.Contains(x.Id) && x.IsPoster == true);

            if (book.FaceImage != null)
            {

                if (book.FaceImage.ContentType != "image/png" && book.FaceImage.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("FaceImage", "ancaq sekil yukle");
                    return View();
                }

                if (book.FaceImage.Length > 1048576)
                {
                    ModelState.AddModelError("FaceImage", "1 mb dan az yukle pul yazir ");
                    return View();
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", book.FaceImage);
                BookImage bookImage = new BookImage
                {
                    book = book,
                    ImageUrl = newFileName,
                    IsPoster = true,
                };
                _context.BookImages.Add(bookImage);
            };


            if (book.BackViewImg != null)
            {

                if (book.BackViewImg.ContentType != "image/png" && book.BackViewImg.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("BackViewImg", "ancaq sekil yuklemek olur");
                    return View();
                }

                if (book.BackViewImg.Length > 1048576)
                {
                    ModelState.AddModelError("BackViewImg", "1 mb dan cox yukleme ");
                    return View();
                }

                string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", book.BackViewImg);
                BookImage bookImage = new BookImage
                {
                    book = book,
                    ImageUrl = newFileName,
                    IsPoster = false,
                };
                _context.BookImages.Add(bookImage);
            };

            if (book.BookDetailImgs != null)
            {
                foreach (var img in book.BookDetailImgs)
                {
                    string fileName = img.FileName;
                    if (img.ContentType != "image/png" && img.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("BookDetailImgs", "ancaq sekilyukle");
                        return View();
                    }

                    if (img.Length > 1048576)
                    {
                        ModelState.AddModelError("BookDetailImgs", "1 mb dan cox yukleme ");
                        return View();
                    }

                    string newFileName = Helper.GetFileName(_env.WebRootPath, "upload", img);
                    BookImage bookImage = new BookImage
                    {
                        book = book,
                        ImageUrl = newFileName,
                        IsPoster = null,
                    };
                    _context.BookImages.Add(bookImage);
                }
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
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (id == null) return NotFound("Error");

            Book book = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);

            if (book == null) return NotFound("Error");


            return View(book);


        }
        [HttpPost]
        public IActionResult Delete(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            var wanted = _context.Books.Include(x=>x.BookImages).FirstOrDefault(x => x.Id == book.Id);
            if (wanted == null) return NotFound();
            _context.Books.Remove(wanted);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}