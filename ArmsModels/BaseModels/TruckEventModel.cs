using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class EventModel
    {
        public EventModel()
        {
            UserInfo = new();
        }

        public long TruckEventID { get; set; }
        public byte EventTypeID { get; set; }       
        public DateTime? EventTime { get; set; }
        public long? EventReading { get; set; }
        public int BranchID { get; set; }
        public int OriginID { get; set; }
        public int DestinationID { get; set; }
        public int TruckID { get; set; }
        public int? DriverID { get; set; }
        public long? TripID { get; set; }
        public long? GcSetID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class EventTypeModel
    {
        public byte EventTypeID { get; set; }
        public string EventTypeName { get; set; }
        public bool IsStationary { get; set; }
        public bool IsGcRelated { get; set; }
        public bool IsTripRelated { get; set; }
        public bool IsDriverRelated { get; set; }
        public bool IsDriverRequired { get; set; }
        public bool IsBlocking { get; set; }
        public int DisplayOrder { get; set; }
        public string  EventStatusText { get; set; }
        public byte LimitPostEvent { get; set; }
    }
}
