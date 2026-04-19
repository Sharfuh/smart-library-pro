using System.ComponentModel.DataAnnotations;

namespace SmartLibraryPro.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public int AvailableQuantity { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}