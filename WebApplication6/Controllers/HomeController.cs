using Microsoft.AspNetCore.Mvc;
using WebApplication6.ViewModels;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context=context;
        }
        public IActionResult Index()
        {
            HomeViewModels homeViewModels = new HomeViewModels()
            {
                Sliders = _context.Slider.ToList(),
                Services = _context.Services.ToList()
            };
            return View(homeViewModels);
        }

     
    }
}