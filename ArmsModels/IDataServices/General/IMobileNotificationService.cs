using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IMobileNotificationService
    {       
        MobileNotificationModel UpdateMobileNotification(MobileNotificationModel model);
        IEnumerable<MobileNotificationModel> SelectAllToken();
    }
}