using lesson11_backend.DAL;
using lesson11_backend.HomeViewModel;
using lesson11_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace lesson11_backend.Controllers
{
    public class HomeController : Controller
    {
        


        private readonly PustokDbContext _context;

        public HomeController(PustokDbContext context)
        {
            _context = context;
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Index()
        {
            var slider = _context.Sliders.FirstOrDefault(x => x.BtnUrl == null);

            ViewModel vm = new ViewModel
            {
                Sliders = _context.Sliders.OrderBy(x => x.Id).ToList(),
                BestSellerBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsBestSeller).ToList(),
                NewBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsNew).ToList(),
                DiscountedBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.DiscountPercent > 0).ToList()
            };
            return View(vm);
        }
        public IActionResult SetSession( string key,string value)
        {   
            HttpContext.Session.SetString(key,value);
            return Content("");
        }
        public IActionResult GetSession(string key)
        {
            var value = HttpContext.Session.GetString(key);
            return Content("");
        }


    }
}