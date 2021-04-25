using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace agent
{
    public class SignalRConnection
    {
        private HubConnection connection = null;

        public Int32 Interval { get; set; } = 60;
        public async void Send(string url, string Message)
        {
            if (connection == null)
            {
                connection = new HubConnectionBuilder()
                    .WithUrl(url)
                    .WithAutomaticReconnect()
                    .Build();

                // receive a message from the hub
                connection.On<string, string>("ReceiveMessage", (user, message) => OnReceiveMessage(user, message));
                connection.On<string>("Notify", (message) => OnNotify( message));
                connection.On<Int32>("ScanInterval", (newInterval) => OnScanInterval(newInterval));
            }
            if (connection.State != HubConnectionState.Connected)
            {

                var t = connection.StartAsync();
                try
                {
                    t.Wait();


                }
                catch (Exception ex)
                {
                    // Console.WriteLine(ex.Message);
                    await connection.DisposeAsync();
                    connection = null;
                    return;
                }


            }
      

            if (connection.State == HubConnectionState.Connected)
            {
                try
                {
                    // send a message to the hub
                    await connection.InvokeAsync("getData", Message);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    await connection.DisposeAsync();
                    connection = null;
                    return;
                }
            }

        }

        public async void Stop()
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
            connection = null;
        }

        private async void OnReceiveMessage(string user, string message)
        {
            Console.WriteLine($"{user}: {message}");
            //await connection.StopAsync();

        }

        private async void OnNotify( string message)
        {
            Console.WriteLine($"{message}");
            //await connection.StopAsync();

        }


        private async void OnScanInterval(Int32 newInterval)
        {
            Interval = newInterval;
            Console.WriteLine($"Interval set to : {Interval} second");

        }

    }
}