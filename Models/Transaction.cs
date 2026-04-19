using System.ComponentModel.DataAnnotations;

namespace SmartLibraryPro.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int MemberId { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public decimal Fine { get; set; } = 0;

        // Navigation Properties
        public Book? Book { get; set; }
        public Member? Member { get; set; }
    }
}