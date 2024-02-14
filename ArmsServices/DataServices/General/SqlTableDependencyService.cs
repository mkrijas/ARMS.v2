using ArmsModels.BaseModels.General;
using ArmsServices;
using ArmsServices.DataServices;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.Abstracts;
using TableDependency.SqlClient.Base.EventArgs;

namespace ArmsServices.DataServices
{
    public class SqlTableDependencyService 
    {
        private const string TableName = "General.Notification";
        private SqlTableDependency<PushNotificationModel> _notifier; 
        private IHubContext<ServerSignalHub> _hubContext;
        
        string ConnectionString { get; set; }
        ILogger<DbService> _logger;
        public string ResultErrorMessage { get; set; } = string.Empty;

        bool Active = false;



        public SqlTableDependencyService(IConfiguration configuration,IHubContext<ServerSignalHub> context , ILogger<DbService> logger )
        {
            this._logger = logger;
            this.ConnectionString = configuration.GetConnectionString("ArmsDB");
            _hubContext = context;           
        }
        public void SubscribeTableDependency()
        {
            if (!Active)
            {
                try
                {
                    _notifier = new SqlTableDependency<PushNotificationModel>(ConnectionString, TableName);
                    _notifier.OnChanged += TabledependencyChange;
                    _notifier.OnError += TabledependencyError;
                    _notifier.Start();
                    Active = true;
                }
                catch (Exception ex)
                {
                    ResultErrorMessage = ex.Message;
                    Console.Write(ResultErrorMessage);
                }
            }
        }
        private async void TabledependencyChange(object obj, RecordChangedEventArgs<PushNotificationModel> e)
        {            
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var changeEntity = e.Entity;
                //if (changeEntity != null && changeEntity.MessageGroupID != null && (changeEntity.MessageGroupID.ToLower().Trim() == ("PeriodicMaintainence").ToLower().Trim() || changeEntity.MessageGroupID.ToLower().Trim() == ("EWayBill").ToLower().Trim()))
                if (changeEntity != null)
                {                    
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Triggered");
                }
            }
        }

        private void TabledependencyError(object obj, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            try
            {

                Exception ex = e.Error;
                throw ex;
            }
            catch (Exception ex)
            {
                ResultErrorMessage = ex.Message;
                Console.Write(ResultErrorMessage);
            }
        }

        public void Dispose()
        {
            _notifier.Stop();
            _notifier.Dispose();
        }
    }
}
