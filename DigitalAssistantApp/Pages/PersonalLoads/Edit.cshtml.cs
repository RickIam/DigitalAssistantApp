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
        public SelectList? TeachersNames { get; set; }



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PersonalLoads == null)
            {
                return NotFound();
            }
            var personalload =  await _context.PersonalLoads.FirstOrDefaultAsync(m => m.PersonalLoadId == id);
            if (personalload == null)
            {
                return NotFound();
            }

            List<Teacher> teachers = _context.Teachers.ToList();
            ViewData["Teachers"] = new SelectList(teachers, "TeacherId", "FullName");

            var loads = await _context.Loads.Where(l => l.PersonalLoadId == personalload.PersonalLoadId).ToListAsync();
            if (loads.Count==0)
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
            }
            PersonalLoad = personalload;
            return Page();
        }

        public IActionResult OnGetAddForm()
        {
            // Добавление новой формы для ввода данных
            Loads.Add(new Load());
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PersonalLoad).State = EntityState.Modified;
            foreach (Load load in Loads)
            {
                if (load.TeacherId != null && load.HoursCount != null)
                {
                    load.PersonalLoadId = PersonalLoad.PersonalLoadId;
                    if (load.LoadId > 0)
                    {
                        _context.Attach(load).State = EntityState.Modified;
                    }
                    else
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
