using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryPro.Data;

namespace SmartLibraryPro.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 📊 BOOK STATS
            var totalBooks = await _context.Books.CountAsync();
            var availableBooks = await _context.Books.SumAsync(b => b.AvailableQuantity);
            var totalQuantity = await _context.Books.SumAsync(b => b.Quantity);
            var issuedBooks = totalQuantity - availableBooks;

            // 👥 MEMBERS
            var totalMembers = await _context.Members.CountAsync();

            // ⚠️ OVERDUE
            var overdueBooks = await _context.Transactions
                .Where(t => t.ReturnDate == null && t.DueDate < DateTime.Now)
                .CountAsync();

            // 🔥 RECENT ACTIVITY (NEW)
            var recentTransactions = await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .OrderByDescending(t => t.IssueDate)
                .Take(5)
                .ToListAsync();

            // 🎯 VIEWBAG
            ViewBag.TotalBooks = totalBooks;
            ViewBag.AvailableBooks = availableBooks;
            ViewBag.IssuedBooks = issuedBooks;
            ViewBag.TotalMembers = totalMembers;
            ViewBag.OverdueBooks = overdueBooks;

            ViewBag.RecentTransactions = recentTransactions; // 🔥 NEW

            return View();
        }
    }
}