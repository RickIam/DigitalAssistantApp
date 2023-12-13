using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalAssistantApp;
using DigitalAssistantApp.DataBaseModels;

namespace DigitalAssistantApp.Pages.Groups
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
        ViewData["SpecId"] = new SelectList(_context.Specialities, "SpecId", "SpecId");
            return Page();
        }

        [BindProperty]
        public Group Group { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Groups == null || Group == null)
            {
                return Page();
            }

            _context.Groups.Add(Group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
