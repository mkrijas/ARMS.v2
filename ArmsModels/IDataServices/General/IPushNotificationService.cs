using ArmsModels.BaseModels.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IPushNotificationService
    {
        string GetMessageTitle(string DocType, string DocNumber, string Varification);
        string GetMessageBody(string DocType, string DocNumber, string Varification, DateTime? DocDate);
        PushNotificationModel UpdatePushNotification(PushNotificationModel model);
        IEnumerable<PushNotificationModel> SelectActiveNotifications(int? BranchID, string UserID = null);
        public IEnumerable<PushNotificationModel> SelectNotificationsBasedOnBranchDocumentIdDocumentTypeId(int? ID, int? DocumentID, int? DocumentTypeID);
        public IEnumerable<PushNotificationModel> SelectActiveNotifications();
        int AknowledgedSelectedItems(string MessageIDs, string UserID);
        int AknowledgedCurrentItem(int? MessageID, string UserID);
        IEnumerable<PushNotificationGroupModel> GetAllGroupList();
    }
}