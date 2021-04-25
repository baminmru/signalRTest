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

        Server.models.myContext dbContext;
      

        public AgentHub(Server.models.myContext _context)
        {
            dbContext = _context;
        }

        

       



        public async Task Send(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, message );
        }

        public async Task getData(string message)
        {
            Console.WriteLine("Received message: " + message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, "Accepted");

            PerfData pd = JsonConvert.DeserializeObject<PerfData>(message);
            pd.PerfDataID = Guid.NewGuid();
            dbContext.PerfData.Add(pd);
            foreach (var di in pd.driverData)
            {
                di.perfDataid = pd.PerfDataID;
                di.PerfDriverInfoid = Guid.NewGuid();
                dbContext.PerfDriverInfo.Add(di);
            }
           
            await dbContext.SaveChangesAsync();
        }

        public override async Task OnConnectedAsync()
        {
            AgentSettings s = dbContext.AgentSettings.SingleOrDefault(a => a.Name == "Scan Interval");
            if (s != null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ScanInterval", s.Value);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ScanInterval", 10);
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Clients.All.SendAsync("Notify", $"{dbContext.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}



