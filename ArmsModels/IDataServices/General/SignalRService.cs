using ArmsModels.BaseModels.General;
using ArmsModels.BaseModels;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class SignalRService
    {
        private HubConnection hubConnection;
        private  List<PushNotificationModel> allNotificationMessages = new List<PushNotificationModel>();
        private List<PushNotificationModel> currentNotificationMessages = new List<PushNotificationModel>();

        public SignalRService(NavigationManager navigationManager, HubConnection _hubconnection)
        {
            hubConnection = _hubconnection;
            //hubConnection = new HubConnectionBuilder().WithUrl(navigationManager.ToAbsoluteUri("/chatHub")).Build();

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
            await hubConnection.StartAsync();
        }

        public List<PushNotificationModel> GetAllNotificationMessages()
        {
            return allNotificationMessages;
        }

        // Other methods to handle connection events and message sending can be added here

        public event Action StateHasChanged;
    }

//public class ChatHubService
//    {
//        private readonly NavigationManager _navigationManager;
//        private readonly HubConnection _hubConnection;
//        private readonly List<PushNotificationModel> _allNotificationMessages;
//        private List<PushNotificationModel> _currentNotificationMessages;

//        public ChatHubService(NavigationManager navigationManager,int? CurrentBranchId)
//        {
//            _navigationManager = navigationManager;
//            _allNotificationMessages = new List<PushNotificationModel>();

//            // Initialize the hub connection
//            _hubConnection = new HubConnectionBuilder()
//                .WithUrl(_navigationManager.ToAbsoluteUri("/chatHub"))
//                .Build();

//            // Set up the "ReceiveMessage" event handler
//            _hubConnection.On<BranchModel, BranchModel, string, string, int, string>("ReceiveMessage", (InitiateBranch, ReceivedBranch, MessageTitle, MessageBody, RedirectedTo, MsgDatest) =>
//            {
//                _allNotificationMessages.Add(new PushNotificationModel()
//                {
//                    MSgId = _allNotificationMessages.Count + 1,
//                    MessageID = 0,
//                    InitiateBranch = InitiateBranch,
//                    ReceivedBranch = ReceivedBranch,
//                    MessageTitle = MessageTitle,
//                    MessageBody = MessageBody,
//                    RedirectedTo = RedirectedTo,
//                    MsgDateString = MsgDatest
//                });


//                _currentNotificationMessages = _allNotificationMessages.Where(s => (s.ReceivedBranch?.BranchID ?? 0) == CurrentBranchId).ToList();

//                // Call StateHasChanged to update the UI
//                InvokeAsync(() => StateHasChanged());
//            });
//        }

//        public void StartHubConnection()
//        {
//            // Start the hub connection
//            _hubConnection.StartAsync();
//        }

//        public void StopHubConnection()
//        {
//            // Stop the hub connection
//            _hubConnection.StopAsync();
//        }

//        // Other methods related to the chat hub functionality can be added here

//        // You may need to pass the "CurrentBranchId" and the "InvokeAsync" method to this class as well.
//    }

}
