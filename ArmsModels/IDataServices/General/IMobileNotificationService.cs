using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IMobileNotificationService
    {        
        MobileNotificationModel UpdateMobileNotification(MobileNotificationModel model);
    }
}