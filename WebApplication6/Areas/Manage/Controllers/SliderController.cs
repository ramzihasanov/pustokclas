using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            string oldFilePath = "C:\\Users\\ll novbe\\Desktop\\secondtask\\WebApplication1\\WebApplication1\\wwwroot\\assets\\images\\" + existSlider.ImageUrl;

            if (slider.formFile != null)
            {

                string newFileName = slider.formFile.FileName;
                if (slider.formFile.ContentType != "image/jpeg" && slider.formFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("FormFile", "ancaq sekil yukle :)");
                }

                if (slider.formFile.Length > 1048576)
                {
                    ModelState.AddModelError("FormFile", "guce salma 1 mb az yukle");
                }

                if (slider.formFile.FileName.Length > 64)
                {
                    newFileName = newFileName.Substring(newFileName.Length - 64, 64);
                }

                newFileName = Guid.NewGuid().ToString() + newFileName;

                string newFilePath = "C:\\Users\\ll novbe\\Desktop\\secondtask\\WebApplication1\\WebApplication1\\wwwroot\\assets\\images\\" + newFileName;
                using (FileStream fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    slider.formFile.CopyTo(fileStream);
                }

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                existSlider.ImageUrl = newFileName;
            }

            existSlider.Title = slider.Title;
            existSlider.Descirption = slider.Descirption;
            existSlider.RedirectorUrl = slider.RedirectorUrl;
            existSlider.RedirectorUrlText = slider.RedirectorUrlText;
            


            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();
            Slider wantedSilider = _context.Slider.FirstOrDefault(s => s.Id == id);

            if (wantedSilider == null) return NotFound();
            _context.Slider.Remove(wantedSilider);
            _context.SaveChanges();
            return Ok();
        }

      
    }
}
