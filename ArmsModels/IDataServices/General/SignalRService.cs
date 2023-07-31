using ArmsModels.BaseModels.General;
using ArmsModels.BaseModels;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsServices.DataServices.General;

namespace ArmsServices.DataServices
{
    public class SignalRService
    {
        private Microsoft.AspNetCore.SignalR.Client.HubConnection hubConnection;
        private List<PushNotificationModel> allNotificationMessages = new List<PushNotificationModel>();
        private List<PushNotificationModel> currentNotificationMessages = new List<PushNotificationModel>();
        IPushNotificationService pushNotificationService;
        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
        public SignalRService(NavigationManager navigationManager, IPushNotificationService _pushNotificationService)
        {
            //hubConnection = _hubconnection;
            hubConnection = new HubConnectionBuilder().WithUrl(navigationManager.ToAbsoluteUri("/chatHub")).Build();
            pushNotificationService = _pushNotificationService;
        }

        public void RegisterReceivedMessage(Action<PushNotificationModel> notificationModel)
        {
            hubConnection.On<PushNotificationModel>
               ("ReceiveMessage", (notificationModel) =>
               {
                   allNotificationMessages.Add(notificationModel);
               });
        }
        public async Task StartAsync()
        {
            if (!IsConnected)
            {
                await hubConnection.StartAsync();

            }
        }
        public async Task Send(PushNotificationModel notificationMessage)
        {

            if (IsConnected)
            {
                PushNotificationModel result = pushNotificationService.UpdatePushNotification(notificationMessage);

                await hubConnection.SendAsync("SendMessages",
                result);

                notificationMessage = new();
            }
        }

        public List<PushNotificationModel> GetAllNotificationMessages()
        {
            return allNotificationMessages;
        }

        // Other methods to handle connection events and message sending can be added here

        public event Action StateHasChanged;
    }


}
