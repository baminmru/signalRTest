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
using Microsoft.AspNetCore.SignalR;

namespace Server.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly myContext _db;
        private readonly IHubContext<AgentHub> _AgentHub;

        public IndexModel(ILogger<IndexModel> logger, myContext db, IHubContext<AgentHub> AgentHub)
        {
            _logger = logger;
            _db = db;
            _AgentHub = AgentHub;
        }

        public void OnGet()
        {
            agentSettings = _db.AgentSettings.SingleOrDefault(a => a.Name == "Scan Interval");
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
        public AgentSettings agentSettings { get; set; }

        public List<Dictionary<string, string>> pd { get; set; }
        

        public async Task<IActionResult> OnPost()
        {
            AgentSettings s = _db.AgentSettings.SingleOrDefault(a => a.Name == "Scan Interval");
            if (s == null)
            {
                return NotFound();
            }
            s.Value = agentSettings.Value;
            await _db.SaveChangesAsync();
            await  _AgentHub.Clients.All.SendAsync("ScanInterval", s.Value);

            return RedirectToPage("Index");
        }
    }
}
