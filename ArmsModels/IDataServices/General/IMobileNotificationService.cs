using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IMobileNotificationService
    {       
        MobileNotificationModel UpdateMobileNotification(MobileNotificationModel model);
        MobileNotificationModel UpdateMobileNotificationMessage(MobileNotificationModel model);
        IEnumerable<MobileNotificationModel> SelectAllToken();
        IEnumerable<MobileNotificationModel> SelectAllMessage(string UserID);
    }
}