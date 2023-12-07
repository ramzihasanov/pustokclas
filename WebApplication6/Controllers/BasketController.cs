using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            string basketListStr = HttpContext.Request.Cookies["BasketItems"];

            if (basketListStr != null)
            {
                basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketListStr);

                basketViewModel = basketViewModels.FirstOrDefault(x => x.BookId == id);

                if (basketViewModel != null)
                {
                    basketViewModel.Count++;
                }
                else
                {
                    basketViewModel = new BasketViewModel()
                    {
                        BookId = id,
                        Count = 1
                    };

                    basketViewModels.Add(basketViewModel);
                }
            }
            else
            {
                basketViewModel = new BasketViewModel()
                {
                    BookId = id,
                    Count = 1
                };

                basketViewModels.Add(basketViewModel);
            }

            basketListStr = JsonConvert.SerializeObject(basketViewModels);

            HttpContext.Response.Cookies.Append("BasketItems", basketListStr);

            return Ok(); //200
        }

        public IActionResult GetBasketItems()
        {
            List<BasketViewModel> basketItemList = new List<BasketViewModel>();

            string basketItemListStr = HttpContext.Request.Cookies["BasketItems"];

            if (basketItemListStr != null)
            {
                basketItemList = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemListStr);
            }

            return Json(basketItemList);
        }

        public async Task<IActionResult> Checkout()
        {
            List<CheckoutViewModel> checkoutItemList = new List<CheckoutViewModel>();
            List<BasketViewModel> basketItemList = new List<BasketViewModel>();
            CheckoutViewModel checkoutItem = null;

            string basketItemListStr = HttpContext.Request.Cookies["BasketItems"];
            if (basketItemListStr != null)
            {
                basketItemList = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketItemListStr);

                foreach (var item in basketItemList)
                {
                    checkoutItem = new CheckoutViewModel
                    {
                        Book = await _bookRepository.GetByIdAsync(x => x.Id == item.BookId),
                        Count = item.Count
                    };
                    checkoutItemList.Add(checkoutItem);
                }
            }

            return View(checkoutItemList);
        }

    }
}
