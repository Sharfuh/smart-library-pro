using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryPro.Data;
using SmartLibraryPro.Models;

namespace SmartLibraryPro.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 👥 LIST ALL MEMBERS
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                .OrderByDescending(m => m.JoinDate)
                .ToListAsync();

            return View(members);
        }

        // ➕ GET: CREATE
        public IActionResult Create()
        {
            return View();
        }

        // ➕ POST: CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                member.JoinDate = DateTime.Now;

                _context.Add(member);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        // ✏️ GET: EDIT
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        // ✏️ POST: EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            if (id != member.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMember = await _context.Members.AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (existingMember == null) return NotFound();

                    // Preserve Join Date
                    member.JoinDate = existingMember.JoinDate;

                    _context.Update(member);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return BadRequest();
                }
            }

            return View(member);
        }

        // ❌ DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // 👁️ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null) return NotFound();

            return View(member);
        }
    }
}