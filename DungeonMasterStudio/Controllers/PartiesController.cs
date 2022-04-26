#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DungeonMasterStudio.Data;
using DungeonMasterStudio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using BCrypt;

namespace DungeonMasterStudio.Controllers
{
    public class PartiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PartiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Parties
        public async Task<IActionResult> Index()
        {
            //_userManager.GetUserId(HttpContext.User)
            string UserId = _userManager.GetUserId(HttpContext.User);
            //_context.Members.Where(x => x.UserId == UserId).ToListAsync();
            
            //List<Member> partyMembers = await (from member in _context.Members where member.UserId == UserId select member).ToListAsync();
            //List<Party> query = await ( from parties in _context.Parties join member in partyMembers on UserId equals member.UserId select parties).ToListAsync();
            //List<Party> parties2 = await (from parties1 in _context.Parties.Where(x => query.Contains(x)) select parties1).ToListAsync();
            ApplicationUser partyMember = _context.Users.Where(x => x.Id == UserId).FirstOrDefault();

            //List<Party> parties = await (from p in _context.Parties.Where(x => x.Members.Contains(partyMember)) select p).ToListAsync();
            //var movies = _db.Movies.Where(p => p.Genres.Intersect(listOfGenres).Any());
            //var movies = _db.Movies.Where(p => p.Genres.Intersect(listOfGenres).Any());
            //var movies = _db.Movies.Where(p => p.Genres.Any(x => listOfGenres.Contains(x));

            List<Party> parties = await _context.Parties.Where(p => p.Members.Contains(partyMember)).ToListAsync();    
            
            return View(parties);
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties
                .FirstOrDefaultAsync(m => m.PartyID == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // GET: Parties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Name, string Password)
        {
            Party party = new Party();
            party.UserID= _userManager.GetUserId(HttpContext.User);
            party.Name=Name;
            party.Password=Password;    

            if (party != null)
            {
                party.Password = BCrypt.Net.BCrypt.HashPassword(party.Password);
                party.Members = new List<ApplicationUser>();
                party.Members.Add(_context.Users.Where(x => x.Id == _userManager.GetUserId(HttpContext.User)).FirstOrDefault());
                _context.Add(party);
                await _context.SaveChangesAsync();
                //Member partyMember = new Member();
                //partyMember.UserId = _userManager.GetUserId(HttpContext.User);
                //_context.Add(partyMember);
                await _context.SaveChangesAsync();
                //Party p = await _context.Parties.Where(x => x.PartyID == x.PartyID).FirstOrDefaultAsync();
               
                //_context.Update(p);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        public async Task<IActionResult> JoinParty()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToParty(string PartyName, string Password )
        {


            if (!string.IsNullOrEmpty(PartyName) && !string.IsNullOrEmpty(Password))
            {
                Party party = await _context.Parties.Where(x => x.Name == PartyName).FirstOrDefaultAsync();
                if (!party.Equals(null))
                {
                    if (BCrypt.Net.BCrypt.Verify(Password,party.Password))
                    {
                        party.Members = new List<ApplicationUser>();
                        party.Members.Add(_context.Users.Where(x => x.Id == _userManager.GetUserId(HttpContext.User)).FirstOrDefault());
                        _context.Update(party);
                        await _context.SaveChangesAsync();
                    }
                   
                }
                
            }
            return View("JoinParty");
        }

        [Route("/Parties/PartyRoom/{PartyID}")]
        public async Task<IActionResult> PartyRoom(int PartyID)
        {
            Party party = await _context.Parties.Where(p => p.PartyID == PartyID).Include(p=>p.Members).FirstOrDefaultAsync();
            return View(party);
        }

        // GET: Parties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            return View(party);
        }

        // POST: Parties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PartyID,UserID,Name")] Party party)
        {
            if (id != party.PartyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(party);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(party.PartyID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Parties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties
                .FirstOrDefaultAsync(m => m.PartyID == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // POST: Parties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(int id)
        {
            return _context.Parties.Any(e => e.PartyID == id);
        }
    }
}
