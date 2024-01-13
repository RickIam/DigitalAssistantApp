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
        //public SelectList? TeachersNames { get; set; }

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

            List<Teacher> teachers = _context.Teachers.ToList();
            ViewData["Teachers"] = new SelectList(teachers, "TeacherId", "FullName");

            var loads = personalload.Loads;
            /*if (loads.Count==0)
            {
                Loads = new List<Load>
                {
                    new Load(), // Первая форма
                };
                Loads.Add(new Load());
                Loads.Add(new Load());
            }
            else
            {
                Loads = loads;
                while(Loads.Count <3) 
                {
                    Loads.Add(new Load());
                }
            }*/
            Loads = loads.ToList();
            if(Loads.Count==0)
            {
                Loads.Add(new Load());
            }
            PersonalLoad = personalload;
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage(new { id });
            }

            _context.Attach(PersonalLoad).State = EntityState.Modified;
            foreach (Load load in Loads)
            {
                load.PersonalLoadId = PersonalLoad.PersonalLoadId;
                if (load.LoadId > 0)
                {
                    if (load.TeacherId == null)
                    {
                        _context.Remove(load);
                    }
                    else
                    {
                        _context.Attach(load).State = EntityState.Modified;
                    }
                }
                else
                {
                    if (load.TeacherId != null && load.HoursCount != null)
                    {
                        _context.Loads.Add(load);
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
