using Microsoft.AspNetCore.Mvc;
using WebApplication6.CustomExceptions.BookException;
using WebApplication6.DAL;
using WebApplication6.Models;
using WebApplication6.Repositories.Interfaces;
using WebApplication6.Services.Interfaces;

namespace WebApplication6.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
     
        private readonly IBookRepository _bookRepository;
        private readonly IBookService _bookService;

        public BookController( IBookRepository bookRepository, IBookService bookService)
        {
           
            _bookRepository = bookRepository;
            _bookService = bookService;
        }
        public async Task<IActionResult> Index()
        {

            var book = await _bookRepository.GetAllAsync();
            return View(book);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();

            if (!ModelState.IsValid) return View(book);

            try
            {
                await _bookRepository.CreateAsync(book);
                await _bookRepository.CommitAsync();
            }
            catch (InvalidAuthorid ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);

            }
            catch (InvalidContenttype ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (InvalidImgSize ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (InvalidImg ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (invalidGenreId ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (InvalidTagId ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (InvalidNullreferance ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();

            if (!ModelState.IsValid) return View();
            var existBook = await _bookRepository.GetByIdAsync(x => x.Id == id, "BookImages");
            return View(existBook);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();

            if (!ModelState.IsValid) return View(book);

            try
            {
                await _bookService.UpdateAsync(book);
            }
            catch (InvalidAuthorid ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);

            }
            catch (InvalidContenttype ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (InvalidImgSize ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (InvalidImg ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (invalidGenreId ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (InvalidTagId ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (InvalidNullreferance ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }

            await _bookRepository.CommitAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();

            if (id == null) return NotFound();

            
              await _bookService.DeleteAsync(id);
            
          
            return Ok();
        }

    }
}