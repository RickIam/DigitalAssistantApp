using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using DigitalAssistantApp.DataBaseModels;

namespace DigitalAssistantApp.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public IndexModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }

        public IList<Teacher> Teacher { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SelectedDepartment { get; set; }
        public SelectList? TeachersDepatrments { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> DeptQuery = (from m in _context.Faculties
                                            orderby m.FacultyName
                                            select m.FacultyName).Distinct();

            var teachers = from m in _context.Teachers
                           select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                teachers = teachers.Where(a => (a.Lastname + " " + a.Firstname + " " + a.PatronymicName).Contains(SearchString));
            }
            if (SelectedDepartment == "null")
            {
                teachers = teachers.Where(b => b.Department == null);
            }
            else if (!string.IsNullOrEmpty(SelectedDepartment))
            {
                teachers = teachers.Where(b => b.Department == SelectedDepartment);
            }

            TeachersDepatrments = new SelectList(await DeptQuery.ToListAsync());
            Teacher = await teachers.ToListAsync();
        }
    }
}
