using lesson11_backend.DAL;
using lesson11_backend.Models;
using lesson11_backend.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace lesson11_backend.Services
{
    public class LayoutService
    {
        private readonly PustokDbContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public LayoutService(PustokDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }
        public IDictionary<string,string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }
        public BasketViewModel GetBasket()
        {
            BasketViewModel basketvm = new BasketViewModel();
            List<BasketCookieItemViewModel> basketitems= new List<BasketCookieItemViewModel>();
            var basketJson = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            if(basketJson!=null)
            {
                basketitems=JsonConvert.DeserializeObject<List<BasketCookieItemViewModel>>(basketJson);
            }

            foreach (var item in basketitems)
            {
                var book = _context.Books.Include(x=>x.BookImages.Where(x=>x.PosterStatus==true)).FirstOrDefault(x => x.Id == item.BookId);
                var price = book.DiscountPercent>0?(book.SalePrice*(100-book.DiscountPercent)/100):book.SalePrice;
                basketvm.BasketItems.Add(new BasketItemViewModel
                {
                    Book = book,
                    Count = item.Count,
                });
                basketvm.TotalPrice += (price * item.Count);
            }

          
            return basketvm;
        }
    }
}
