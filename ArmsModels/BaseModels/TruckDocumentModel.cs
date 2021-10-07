using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TruckDocumentTypeModel
    {
        public TruckDocumentTypeModel()
        {
            UserInfo = new();
        }

        public int? DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }        
        public int? WarnBefore { get; set; }
        public int? BlockAfter { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class TruckDocumentModel
    {
        public TruckDocumentModel()
        {
            UserInfo = new();
        }
        public string AttachedDocument { get; set; }
        public int? DocumentID { get; set; }
        public int? DocumentTypeID { get; set; }
        public int? TruckID { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }        
        public int? NotificationID { get; set; }        
        public UserInfoModel UserInfo { get; set; }
        public virtual string DocumentTypeName { get; set; }
    }


    public class TruckNotificationModel
    {
        public TruckNotificationModel()
        {
            UserInfo = new();
        }
        public string NotificationText { get; set; }
        public int? NotificationID { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class TruckServiceModel
    {
        public TruckServiceModel()
        {
            UserInfo = new();
        }
        public int? StartKM { get; set; }
        public int? EndKM { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

}
