using lesson11_backend.Models;

namespace lesson11_backend.HomeViewModel
{
    public class ViewModel
    {
       
        
            public List<Slider> Sliders { get; set; }
            public List<Book> BestSellerBooks { get; set; }
            public List<Book> NewBooks { get; set; }
            public List<Book> DiscountedBooks { get; set; }

        
    }
}
