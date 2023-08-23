using ArmsModels.BaseModels.General;
using ArmsServices.DataServices;
using Core.IDataServices.General;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.Abstracts;
using TableDependency.SqlClient.Base.EventArgs;

namespace DAL.DataServices.General
{
    public class SqlTableDependencyService : ISqlTableDependencyService
    {
        SqlTableDependency<PushNotificationModel> tableDependency;
        private Microsoft.AspNetCore.SignalR.Client.HubConnection hubConnection;
        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;

        private readonly IServiceProvider serviceProvider;
        public SqlTableDependencyService(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;

            using (var scope = serviceProvider.CreateScope())
            {
                //var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();
                //var scopedService = scope.ServiceProvider.GetRequiredService<SignalRService(navigationService,) > ();
                // Use the scoped service here
                //scopedService.SendWithOutSave(changeEntity);
                hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:35411/chatHub").Build();
                if (!IsConnected)
                {
                    hubConnection.StartAsync();
                }
            }

        }
        public void SubscribeTableDependency(string ConnectionString)
        {
            tableDependency = new SqlTableDependency<PushNotificationModel>(ConnectionString, "General.Notification");

            tableDependency.OnChanged += TabledependencyChange;
            tableDependency.OnError += TabledependencyError;
            tableDependency.Start();

        }

        private async void TabledependencyChange(object obj, RecordChangedEventArgs<PushNotificationModel> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var changeEntity = e.Entity;
                if (changeEntity != null && changeEntity.MessageGroupID != null && changeEntity.MessageGroupID.ToLower().Trim() == ("PeriodicMaintainence").ToLower().Trim())
                {
                    changeEntity.InitiateBranch = new();
                    changeEntity.InitiateBranch.BranchID = changeEntity.InitiateBranchID;
                    changeEntity.ReceivedBranch = new();
                    changeEntity.ReceivedBranch.BranchID = changeEntity.ReceivedBranchID;
                    if (!IsConnected)
                    {
                        await hubConnection.StartAsync();
                    }
                    await hubConnection.SendAsync("SendMessages",
                        changeEntity);
                }

            }

        }

        private void TabledependencyError(object obj, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Exception ex = e.Error;
            throw ex;
        }
    }
}
