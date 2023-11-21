using Microsoft.AspNetCore.Mvc;

namespace WebApplication6.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DashBoardController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
