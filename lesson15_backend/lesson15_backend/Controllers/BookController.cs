using lesson11_backend.DAL;
using lesson11_backend.Models;
using lesson11_backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace lesson11_backend.Controllers
{
    public class BookController : Controller
    {
        private PustokDbContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public BookController(PustokDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor= httpContextAccessor;
            _context = context;
        }
        public IActionResult Detail(int id)
        {
            Book book = _context.Books
                .Include(x=>x.BookImages)
                .Include(x=>x.BookTags).ThenInclude(x=>x.Tag)
                .Include(x=>x.Author)
                .Include(x=>x.Genre)
                .FirstOrDefault(x=>x.Id == id);
            return View(book);
        }

        public IActionResult GetBookModal(int id)
        {
            var book = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.BookImages)
                .FirstOrDefault(x => x.Id == id);

            return PartialView("BookModalView", book);
        }
        public IActionResult AddToBasket(int id) 
        {
            List<BasketCookieItemViewModel> basketitems;
            var basket = HttpContext.Request.Cookies["basket"];

            if(basket == null)
                basketitems=new List<BasketCookieItemViewModel>();
            else
                basketitems=JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basket);
            
            var wantedbook = basketitems.FirstOrDefault(x=>x.BookId==id);

            if (wantedbook == null)
                basketitems.Add(new BasketCookieItemViewModel { BookId = id, Count = 1 });
            else
                wantedbook.Count++;

            HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketitems));
            BasketViewModel basketvm = new BasketViewModel();
            foreach (var item in basketitems)
            {
                var book = _context.Books.Include(x => x.BookImages.Where(x => x.PosterStatus == true)).FirstOrDefault(x => x.Id == item.BookId);
                var price = book.DiscountPercent > 0 ? (book.SalePrice * (100 - book.DiscountPercent) / 100) : book.SalePrice;
                basketvm.BasketItems.Add(new BasketItemViewModel
                {
                    Book = book,
                    Count = item.Count,
                });
                basketvm.TotalPrice += (price * item.Count);
            }
            return PartialView("BasketCartPartial", basketvm);
            
        }
        public IActionResult RemoveBasket(int id)
        {
            
            var basket = Request.Cookies["basket"];
            if(basket==null)
                return NotFound();
                
            List<BasketCookieItemViewModel> basketitem = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basket);

            BasketCookieItemViewModel item = basketitem.Find(x=>x.BookId==id);

            if (item==null)
                return NotFound();

            basketitem.Remove(item);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketitem));
            decimal totalPrice = 0;
            foreach (var bi in basketitem)
            {
                var book = _context.Books.Include(x => x.BookImages.Where(x => x.PosterStatus == true)).FirstOrDefault(x => x.Id == bi.BookId);
                var price = book.DiscountPercent > 0 ? (book.SalePrice * (100 - book.DiscountPercent) / 100) : book.SalePrice;
                totalPrice += (price * bi.Count);
            }

            return Ok(new { count = basketitem.Count, totalPrice = totalPrice.ToString("0.00") });
        }
        public List<BasketCookieItemViewModel> ShowBasket()
        {
            List<BasketCookieItemViewModel> basketItems = new List<BasketCookieItemViewModel>();
            var basketJson = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            if (basketJson != null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basketJson);
            }

            return basketItems;
        }

    }
}
