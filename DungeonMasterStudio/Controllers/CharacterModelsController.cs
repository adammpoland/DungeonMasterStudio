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

namespace DungeonMasterStudio.Controllers
{
    public class CharacterModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharacterModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CharacterModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Characters.ToListAsync());
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
            return View();
        }

        // POST: CharacterModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Class,Level,Race,Age1,ArmorClass,CurrentHitPoints,MaxHitPoints,Strength,Dexterity,Constitution,Intelligence,Wisdom,Charisma,BackStory,Notes,Date")] CharacterModel characterModel)
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
