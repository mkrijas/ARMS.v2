using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class NotificationModel
    {
        // Model representing a notification
        public NotificationModel()
        {
            UserInfo = new();
        }

        public int? NotificationID { get; set; } // Unique identifier for the notification
        public bool Status { get; set; }
        public DateTime? TriggeredOn { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int? Severity { get; set; }
        public string OriginRef { get; set; }        
        public UserInfoModel UserInfo { get; set; }        
    }    

    public class MobileNotificationModel
    {
        public int? NotificationID { get; set; }
        public string UserID { get; set; }
        public string Token { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; } = false;
    }
}



