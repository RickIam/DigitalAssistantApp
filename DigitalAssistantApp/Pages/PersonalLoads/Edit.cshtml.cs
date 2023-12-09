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
            PersonalLoad = personalload;
           ViewData["PersonalLoadId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId");
           ViewData["PersonalLoadId"] = new SelectList(_context.EducPlans, "EducPlanId", "EducPlanId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PersonalLoad).State = EntityState.Modified;

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
