using System.ComponentModel.DataAnnotations;

namespace lesson11_backend.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
