using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Server.models;
using Newtonsoft.Json;

namespace Server
{
    public class AgentHub : Hub
    {

        Server.models.myContext context;

        public AgentHub(Server.models.myContext _context)
        {
            context = _context;
            agentsettings s = context.agentsettings.SingleOrDefault(a => a.scaninterval > 0);
            if (s != null)
                _Interval = s.scaninterval;

        }

        

        private Int32 _Interval = 0;
        public Int32 Interval
        {
            get { return this._Interval; }
            set { 
                if(this._Interval != value && value >0)
                {
                   this._Interval = value;
                   agentsettings s = context.agentsettings.SingleOrDefault(a => a.scaninterval > 0);
                   if (s != null) {
                        s.scaninterval = value;
                        context.SaveChangesAsync();
                   }
                   

                }
                
            }
        }



        public async Task Send(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, message );
        }

        public async Task getData(string message)
        {
            Console.WriteLine("Received message: " + message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, "Accepted");

            perfdata pd = JsonConvert.DeserializeObject<perfdata>(message);
            pd.perfdataid = Guid.NewGuid();
            context.perfdata.Add(pd);
            foreach (var di in pd.driverdata)
            {
                di.perfdataid = pd.perfdataid;
                di.perfdriverinfoid = Guid.NewGuid();
                context.perfdriverinfo.Add(di);
            }
           
            await context.SaveChangesAsync();
        }

        public override async Task OnConnectedAsync()
        {

            await Clients.All.SendAsync("ScanInterval", Interval);
            await base.OnConnectedAsync();
            Console.WriteLine("send scan interval=" + Interval.ToString());



        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}



