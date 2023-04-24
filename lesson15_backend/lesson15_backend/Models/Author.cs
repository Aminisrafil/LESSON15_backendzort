using System.ComponentModel.DataAnnotations;

namespace lesson11_backend.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(35)]
        public string FullName { get; set; }

        public string BirtYear { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
