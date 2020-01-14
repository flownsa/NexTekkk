using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nextekk.Models;

namespace Nextekk.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly Nextekk.Models.NextekkDBContext _context;

        public IndexModel(Nextekk.Models.NextekkDBContext context)
        {
            _context = context;
        }

        public IList<Employee> Employee { get;set; }

        public async Task OnGetAsync()
        {
            Employee = await _context.Employees.ToListAsync();
        }
    }
}
