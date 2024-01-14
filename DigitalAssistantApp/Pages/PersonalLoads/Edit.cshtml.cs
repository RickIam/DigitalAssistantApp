using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalAssistantApp;
using DigitalAssistantApp.DataBaseModels;
using NuGet.Packaging.Signing;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using System.ComponentModel.DataAnnotations;

namespace DigitalAssistantApp.Pages.PersonalLoads
{
    public class EditModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public EditModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }
        [BindProperty]
        public PersonalLoad PersonalLoad { get; set; } = default!;
        [BindProperty]
        public List<Load> Loads { get; set; }
        [BindProperty]
        public float MaxHour { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PersonalLoads == null)
            {
                return NotFound();
            }
            var personalload = await _context.PersonalLoads
                .Include(t => t.Loads)
                .Include(p => p.EducPlan)
                .ThenInclude(b => b.Subject)
                .Include(c => c.EducPlan)
                .ThenInclude(d => d.Stream)
                .FirstOrDefaultAsync(m => m.PersonalLoadId == id);
            if (personalload == null)
            {
                return NotFound();
            }
            var loads = personalload.Loads;
            Loads = loads.ToList();
            if(Loads.Count==0)
            {
                Loads.Add(new Load());
            }
            PersonalLoad = personalload;
            PersonalLoad.EducPlan = personalload.EducPlan;
            MaxHour = (float)PersonalLoad.EducPlan.H;
            List<Teacher> teachers = _context.Teachers.ToList();
            ViewData["Teachers"] = new SelectList(teachers, "TeacherId", "FullName");
            return Page();
        }


        public async Task<IActionResult> OnGetDeleteLoadAsync(int? id, int? load_id)
        {
            if (id == null || load_id == null)
            {
                return NotFound();
            }

            var load = await _context.Loads.FindAsync(load_id);

            if (load != null)
            {
                _context.Remove(load);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { id });
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage(new { id });
            }
            _context.Attach(PersonalLoad).State = EntityState.Modified;
            for (int i =0; i < Loads.Count; i++)
            {
                if (Loads[i].HoursCount > MaxHour)
                {
                    List<Teacher> teachers = _context.Teachers.ToList();
                    ViewData["Teachers"] = new SelectList(teachers, "TeacherId", "FullName");
                    ModelState.AddModelError("Loads["+i+"].HoursCount", $"Значение не может быть больше {MaxHour} часов.");
                    return Page();
                }
                Loads[i].PersonalLoadId = PersonalLoad.PersonalLoadId;
                if (Loads[i].LoadId > 0)
                {
                    if (Loads[i].TeacherId == null)
                    {
                        _context.Remove(Loads[i]);
                    }
                    else
                    {
                        _context.Attach(Loads[i]).State = EntityState.Modified;
                    }
                }
                else
                {
                    if (Loads[i].TeacherId != null && Loads[i].HoursCount != null)
                    {
                        _context.Loads.Add(Loads[i]);
                    }
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalLoadExists(PersonalLoad.PersonalLoadId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }

        private bool PersonalLoadExists(int id)
        {
            return (_context.PersonalLoads?.Any(e => e.PersonalLoadId == id)).GetValueOrDefault();
        }
    }
}
