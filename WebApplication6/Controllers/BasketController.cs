using Microsoft.AspNetCore.Mvc;
using WebApplication6.Repositories.Interfaces;
using WebApplication6.ViewModels;

namespace WebApplication6.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BasketController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public BasketController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddToBasket(int id)
        {
            if(!_bookRepository.Table.Any(x=>x.Id==id )) return NotFound();
            List<BasketViewModel> basketViewModels = new List<BasketViewModel>();
            BasketViewModel basketViewModel = null;

        }



        public IActionResult GetBasketItem(int id)
        {

        }
    }
}
