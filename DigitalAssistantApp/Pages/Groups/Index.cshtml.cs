using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DigitalAssistantApp;
using DigitalAssistantApp.DataBaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DigitalAssistantApp.Pages.Groups
{
    public class IndexModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public IndexModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }

        public IList<Group> Group { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; } 
        [BindProperty(SupportsGet = true)]
        public Speciality? SelectedSpeciality { get; set; } 
        public SelectList? GroupsSpeciality { get; set; }
        public async Task OnGetAsync()
        {

                IQueryable<Speciality> SpecQuery = (from m in _context.Groups
                                                    orderby m.Speciality
                                                    select m.Speciality).Distinct();
                var groups = from m in _context.Groups
                             select m;

                if (!string.IsNullOrEmpty(SearchString))
                {
                    groups = groups.Where(a => a.GroupNumber.Contains(SearchString));

                }

                GroupsSpeciality = new SelectList(await SpecQuery.ToListAsync());
                Group = await groups.ToListAsync();
            
        }
    }
}
