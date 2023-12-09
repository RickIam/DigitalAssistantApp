using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DigitalAssistantApp;
using DigitalAssistantApp.DataBaseModels;

namespace DigitalAssistantApp.Pages.PersonalLoads
{
    public class DeleteModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public DeleteModel(DigitalAssistantApp.DadContext context)
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

            var personalload = await _context.PersonalLoads.FirstOrDefaultAsync(m => m.PersonalLoadId == id);

            if (personalload == null)
            {
                return NotFound();
            }
            else 
            {
                PersonalLoad = personalload;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.PersonalLoads == null)
            {
                return NotFound();
            }
            var personalload = await _context.PersonalLoads.FindAsync(id);

            if (personalload != null)
            {
                PersonalLoad = personalload;
                _context.PersonalLoads.Remove(PersonalLoad);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
