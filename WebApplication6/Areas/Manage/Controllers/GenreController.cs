using Microsoft.AspNetCore.Mvc;


namespace WebApplication6.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class GenreController : Controller
    {
        private readonly AppDbContext _context;

        public GenreController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var genres = _context.Genres.ToList();
            return View(genres);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            if (!ModelState.IsValid) return View(genre);
            if (_context.Genres.Any(x => x.Name.ToLower().Trim() == genre.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", "Genre alredy exist!!!");
                return View(genre);
            }
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Genre wanted = _context.Genres.FirstOrDefault(x => x.Id == id);
            if (wanted == null) return NotFound();
            return View(wanted);
        }
        [HttpPost]
        public IActionResult Update(Genre genre)
        {
            Genre existGenre = _context.Genres.FirstOrDefault(x => x.Id == genre.Id);
            if (existGenre == null) return NotFound();
            if (!ModelState.IsValid) return View(existGenre);
            if (_context.Genres.Any(x => x.Id != genre.Id && x.Name.ToLower().Trim() == genre.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", "Genre alredy exist!!!");
                return View(genre);
            }
            existGenre.Name = genre.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var wanted = _context.Genres.FirstOrDefault(x => x.Id == id);
            if (wanted == null) return NotFound();
            return View(wanted);
        }
        [HttpPost]
        public IActionResult Delete(Genre genre)
        {
            var wanted = _context.Genres.FirstOrDefault(x => x.Id == genre.Id);
            if (wanted == null) return NotFound();
            _context.Genres.Remove(wanted);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}