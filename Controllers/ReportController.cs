using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryPro.Data;

namespace SmartLibraryPro.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📊 DASHBOARD REPORT
        public async Task<IActionResult> Index()
        {
            var totalBooks = await _context.Books.CountAsync();
            var totalMembers = await _context.Members.CountAsync();

            var issuedBooks = await _context.Transactions
                .CountAsync(t => t.ReturnDate == null);

            var overdueBooks = await _context.Transactions
                .CountAsync(t => t.ReturnDate == null && t.DueDate < DateTime.Now);

            var totalFine = await _context.Transactions
                .SumAsync(t => t.Fine);

            var recentTransactions = await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .OrderByDescending(t => t.IssueDate)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalBooks = totalBooks;
            ViewBag.TotalMembers = totalMembers;
            ViewBag.IssuedBooks = issuedBooks;
            ViewBag.OverdueBooks = overdueBooks;
            ViewBag.TotalFine = totalFine;
            ViewBag.RecentTransactions = recentTransactions;

            return View();
        }
    }
}