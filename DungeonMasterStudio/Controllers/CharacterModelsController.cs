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
using DungeonMasterStudio.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace DungeonMasterStudio.Controllers
{
    public class CharacterModelsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CharacterModelsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        // GET: CharacterModels
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Characters.Where(x => x.UserID == _userManager.GetUserId(HttpContext.User)).ToListAsync());
        }

        // GET: CharacterModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterModel = await _context.Characters
                .FirstOrDefaultAsync(m => m.ID == id);
            if (characterModel == null)
            {
                return NotFound();
            }

            return View(characterModel);
        }

        // GET: CharacterModels/Create
        public IActionResult Create()
        {
            Random rnd = new Random();
            ViewData["Class"] = RandomLists.GetClasses()[rnd.Next(0, RandomLists.GetClasses().Count)];
            ViewData["Age"] = RandomLists.GetAges()[rnd.Next(0, RandomLists.GetAges().Count)];
            ViewData["Race"] = RandomLists.GetRaces()[rnd.Next(0, RandomLists.GetRaces().Count)];
            ViewData["Name"] = RandomLists.GetFirstNames()[rnd.Next(0, RandomLists.GetFirstNames().Count)] +" "+ RandomLists.GetLastNames()[rnd.Next(0, RandomLists.GetLastNames().Count )];

            ViewData["Strength"] = GetModifier(GetAtribute()).ToString();
            ViewData["Dexterity"] = GetModifier(GetAtribute()).ToString();
            ViewData["Intelligence"] = GetModifier(GetAtribute()).ToString();
            ViewData["Constitution"] = GetModifier(GetAtribute()).ToString();
            ViewData["Wisdom"] = GetModifier(GetAtribute()).ToString();
            ViewData["Charisma"] = GetModifier(GetAtribute()).ToString();
            return View();
        }
        private int GetAtribute()
        {
            Dice die1 = new Dice(6);
            Dice die2 = new Dice(6);
            Dice die3 = new Dice(6);
            Dice die4 = new Dice(6);
            int sum = 0;


            List<int> Rolls = new List<int>();
            Rolls.Add(die1.Roll());
            Rolls.Add(die2.Roll());
            Rolls.Add(die3.Roll());
            Rolls.Add(die4.Roll());

            for (int i = 0; i < Rolls.Count; i++)
            {
                if (Rolls[i] == Rolls.Min())
                {
                    Rolls.RemoveAt(i);
                    break;
                }

            }


            for (int i = 0; i < Rolls.Count; i++)
            {
                sum += Rolls[i];
            }

            return sum;
        }

        private int GetModifier(int sum)
        {
            if (sum == 1)
            {
                return -5;
            }
            else if (sum == 2 || sum == 3)
            {
                return -4;
            }
            else if (sum == 4 || sum == 5)
            {
                return -3;
            }
            else if (sum == 6 || sum == 7)
            {
                return -2;
            }
            else if (sum == 8 || sum == 9)
            {
                return -1;
            }
            else if (sum == 10 || sum == 11)
            {
                return 0;
            }
            else if (sum == 12 || sum == 13)
            {
                return 1;
            }
            else if (sum == 14 || sum == 15)
            {
                return 2;
            }
            else if (sum == 16 || sum == 17)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        // POST: CharacterModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharacterModel characterModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characterModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(characterModel);
        }

        // GET: CharacterModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterModel = await _context.Characters.FindAsync(id);
            if (characterModel == null)
            {
                return NotFound();
            }
            return View(characterModel);
        }

        // POST: CharacterModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Class,Level,Race,Age1,ArmorClass,CurrentHitPoints,MaxHitPoints,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma,BackStory,Notes,Date")] CharacterModel characterModel)
        {
            if (id != characterModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterModelExists(characterModel.ID))
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
            return View(characterModel);
        }

        // GET: CharacterModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterModel = await _context.Characters
                .FirstOrDefaultAsync(m => m.ID == id);
            if (characterModel == null)
            {
                return NotFound();
            }

            return View(characterModel);
        }

        // POST: CharacterModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterModel = await _context.Characters.FindAsync(id);
            _context.Characters.Remove(characterModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterModelExists(int id)
        {
            return _context.Characters.Any(e => e.ID == id);
        }
    }
}
