using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryPro.Data;
using SmartLibraryPro.Models;

namespace SmartLibraryPro.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📋 ALL TRANSACTIONS
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .OrderByDescending(t => t.IssueDate)
                .ToListAsync();

            return View(transactions);
        }

        // 📤 ISSUE BOOK - GET
        public async Task<IActionResult> Issue()
        {
            ViewBag.Books = await _context.Books
                .Where(b => b.AvailableQuantity > 0)
                .ToListAsync();

            ViewBag.Members = await _context.Members.ToListAsync();

            return View();
        }

        // 📤 ISSUE BOOK - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Issue(int bookId, int memberId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null || book.AvailableQuantity <= 0)
            {
                TempData["Error"] = "Book not available!";
                return RedirectToAction(nameof(Issue));
            }

            var transaction = new Transaction
            {
                BookId = bookId,
                MemberId = memberId,
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                Fine = 0
            };

            // Reduce stock
            book.AvailableQuantity--;

            _context.Transactions.Add(transaction);
            _context.Books.Update(book);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // 📥 RETURN BOOK - GET
        public async Task<IActionResult> Return(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null) return NotFound();

            return View(transaction);
        }

        // 📥 RETURN BOOK - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfirmed(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Book)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null) return NotFound();

            if (transaction.ReturnDate != null)
            {
                TempData["Error"] = "Book already returned!";
                return RedirectToAction(nameof(Index));
            }

            transaction.ReturnDate = DateTime.Now;

            // Fine Calculation
            if (transaction.ReturnDate > transaction.DueDate)
            {
                int daysLate = (transaction.ReturnDate.Value - transaction.DueDate).Days;
                transaction.Fine = daysLate * 5;
            }

            // Increase stock
            transaction.Book!.AvailableQuantity++;

            _context.Update(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // 📊 OVERDUE LIST
        public async Task<IActionResult> Overdue()
        {
            var overdue = await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .Where(t => t.ReturnDate == null && t.DueDate < DateTime.Now)
                .ToListAsync();

            return View(overdue);
        }

        // 👁️ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null) return NotFound();

            return View(transaction);
        }
    }
}