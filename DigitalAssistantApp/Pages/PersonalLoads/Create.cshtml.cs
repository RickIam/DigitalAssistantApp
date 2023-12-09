using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalAssistantApp;
using DigitalAssistantApp.DataBaseModels;

namespace DigitalAssistantApp.Pages.PersonalLoads
{
    public class CreateModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public CreateModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["PersonalLoadId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId");
        ViewData["PersonalLoadId"] = new SelectList(_context.EducPlans, "EducPlanId", "EducPlanId");
            return Page();
        }

        [BindProperty]
        public PersonalLoad PersonalLoad { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.PersonalLoads == null || PersonalLoad == null)
            {
                return Page();
            }

            _context.PersonalLoads.Add(PersonalLoad);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
