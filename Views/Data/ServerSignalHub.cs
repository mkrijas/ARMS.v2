using ArmsModels.BaseModels;
using ArmsModels.BaseModels.General;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Views.Data
{
    public class ServerSignalHub : Hub
    {      

        public async Task SendMessages(PushNotificationModel notificationModel)
        {
            await Clients.All.SendAsync("ReceiveMessage", notificationModel);
        }


        public async Task SendMessage(string user, string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message);
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}