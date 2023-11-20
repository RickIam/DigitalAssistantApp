using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DigitalAssistantApp;

namespace DigitalAssistantApp.Pages.Teachers
{
    public class DetailsModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public DetailsModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }

      public Teacher Teacher { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }
            else 
            {
                Teacher = teacher;
            }
            return Page();
        }
    }
}
