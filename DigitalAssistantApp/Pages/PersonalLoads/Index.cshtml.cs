﻿using System;
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
    public class IndexModel : PageModel
    {
        private readonly DigitalAssistantApp.DadContext _context;

        public IndexModel(DigitalAssistantApp.DadContext context)
        {
            _context = context;
        }
        public IList<PersonalLoad> PersonalLoad { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.PersonalLoads != null)
            {
                PersonalLoad = await _context.PersonalLoads
                .Include(p => p.PersonalLoad1)
                .Include(p => p.PersonalLoadNavigation).ToListAsync();
            }
        }
    }
}