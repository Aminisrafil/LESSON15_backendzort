namespace lesson11_backend.Models
{
    public class BookTag
    {
        public int TagId { get; set; }
        public int BookId { get; set; }

        public Tag Tag { get; set; }
        public Book Book { get; set; }
    }
}
