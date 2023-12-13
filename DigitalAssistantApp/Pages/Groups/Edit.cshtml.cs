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

namespace DigitalAssistantApp.Pages.Groups
{
    public class EditModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public EditModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Group Group { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group =  await _context.Groups.FirstOrDefaultAsync(m => m.GroupNumber == id);
            if (group == null)
            {
                return NotFound();
            }
            Group = group;
           ViewData["SpecId"] = new SelectList(_context.Specialities, "SpecId", "SpecId");
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

            _context.Attach(Group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(Group.GroupNumber))
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

        private bool GroupExists(string id)
        {
          return (_context.Groups?.Any(e => e.GroupNumber == id)).GetValueOrDefault();
        }
    }
}
