using Microsoft.AspNetCore.Mvc;
using WebApplication6.DAL;
using WebApplication6.Models;

namespace WebApplication6.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Slider> sliders = _context.Slider.ToList();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _context.Slider.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Update(int id)
        {
            Slider wantedSilider = _context.Slider.FirstOrDefault(s => s.Id == id);

            if (wantedSilider == null) return NotFound();

            return View(wantedSilider);
        }

        [HttpPost]
        public IActionResult Update(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            Slider existSlider = _context.Slider.FirstOrDefault(x => x.Id == slider.Id);

            if (existSlider == null) return NotFound();

            existSlider.Title = slider.Title;
            existSlider.Descirption = slider.Descirption;
            existSlider.RedirectorUrl = slider.RedirectorUrl;
            existSlider.RedirectorUrlText = slider.RedirectorUrlText;
            existSlider.ImageUrl = slider.ImageUrl;


            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Slider wantedSilider = _context.Slider.FirstOrDefault(s => s.Id == id);

            if (wantedSilider == null) return NotFound();

            return View(wantedSilider);
        }

        [HttpPost]
        public IActionResult Delete(Slider slider)
        {
            var existSilider = _context.Services.FirstOrDefault(x => x.Id == slider.Id);

            if (existSilider == null)
            {
                return NotFound();
            }

            _context.Services.Remove(existSilider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
