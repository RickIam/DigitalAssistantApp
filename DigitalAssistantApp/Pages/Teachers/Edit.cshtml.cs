﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalAssistantApp.DataBaseModels;

namespace DigitalAssistantApp.Pages.Teachers
{
    public class EditModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;
        public SelectList? TeachersDepatrments { get; set; }

        public EditModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher =  await _context.Teachers.FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }
            IQueryable<string> DeptQuery = (from m in _context.Faculties
                                            orderby m.FacultyName
                                            select m.FacultyName).Distinct();
            TeachersDepatrments = new SelectList(await DeptQuery.ToListAsync());
            Teacher = teacher;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(Teacher.TeacherId))
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

        private bool TeacherExists(int id)
        {
          return (_context.Teachers?.Any(e => e.TeacherId == id)).GetValueOrDefault();
        }
    }
}
