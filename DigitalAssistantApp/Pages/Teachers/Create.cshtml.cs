using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalAssistantApp.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssistantApp.Pages.Teachers
{
    public class CreateModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;
        public SelectList? TeachersDepatrments { get; set; }

        public CreateModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            IQueryable<string> DeptQuery = (from m in _context.Faculties
                                            orderby m.FacultyName
                                            select m.FacultyName).Distinct();
            TeachersDepatrments = new SelectList(await DeptQuery.ToListAsync());
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Teachers == null || Teacher == null)
            {
                return Page();
            }

            _context.Teachers.Add(Teacher);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
