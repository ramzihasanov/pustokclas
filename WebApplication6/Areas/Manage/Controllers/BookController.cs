using Microsoft.AspNetCore.Mvc;
using WebApplication6.CustomExceptions.BookException;
using WebApplication6.Services.Interfaces;

namespace WebApplication6.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        public async Task<IActionResult> Index()
        {
            List<Book> Books = await _bookService.GetAllAsync();

            return View(Books);

        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();

            if (!ModelState.IsValid) return View();
            try
            {
                await _bookService.CreateAsync(book);
            }
            catch (InvalidContenttype ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImgSize ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImg ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (invalidGenreId ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidTagId ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidAuthorid ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();

            if (id == null) return NotFound();

            try
            {
                await _bookService.DeleteAsync(id);
            }
            catch (InvalidNullreferance)
            {

            }

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();

            if (id == null) return NotFound();

            Book book = await _bookService.GetAsync(id);

            if (book == null) return NotFound();

            book.TagIds = book.BookTags.Select(t => t.TagId).ToList();


            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {

            ViewBag.Authors = await _bookService.GetAllAuthorAsync();
            ViewBag.Genres = await _bookService.GetAllGenreAsync();
            ViewBag.Tags = await _bookService.GetAllTagAsync();


            try
            {
                await _bookService.UpdateAsync(book);
            }
            catch (InvalidContenttype ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImgSize ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidImg ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (InvalidNullreferance)
            {

            }

            return RedirectToAction("Index");
        }
    }
}