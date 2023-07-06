using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.General
{
    public interface IPushNotificationService
    {
        PushNotificationModel UpdatePushNotification(PushNotificationModel model);
        IEnumerable<PushNotificationModel> SelectUnAknowledgedAndNonTimeElapsedNotifications(int? ID);
        int AknowledgedSelectedItems(string MessageIDs, string UserID);
        int AknowledgedCurrentItem(int? MessageID, string UserID);
    }
}