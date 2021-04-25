using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Server.models;
using System.Data;

namespace Server.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly myContext _db;

        public IndexModel(ILogger<IndexModel> logger, myContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet()
        {
            agentSettings = _db.agentsettings.SingleOrDefault(a => a.scaninterval > 0);
            string SQL= @"select p.name, p.ip,p.ramtotal,p.ramfree, pi.name driver,pi.totalsize, pi.freespace, pi.freeuserspace from perfdata p,
            (
            select max(created) cr, name from perfdata 
            group by name
            ) last,
            perfdriverinfo pi 
            where p.created = last.cr and p.Name = last.name and  p.perfdataid =pi.perfdataid order by p.name, pi.name";

               pd = _db.GetRaw(SQL);
          

        }


        [BindProperty]
        public agentsettings agentSettings { get; set; }

        public List<Dictionary<string, string>> pd { get; set; }
        

        public async Task<IActionResult> OnPost()
        {
            agentsettings s = _db.agentsettings.SingleOrDefault(a => a.scaninterval > 0);
            if (s == null)
            {
                return NotFound();
            }
            s.scaninterval = agentSettings.scaninterval;

            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
