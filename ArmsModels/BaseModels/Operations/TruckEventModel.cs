using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public long? TruckEventID { get; set; }
        [Required]
        public byte? EventTypeID { get; set; }
        [Required]
        public byte? NextEventTypeID { get; set; }
        [Required]
        public DateTime? EventTime { get; set; } = DateTime.Now;


        ///////////////
        
        [Required]
        [Notless("TruckID","EventTime")]

        ///////////////
        

        public long? EventReading { get; set; }
        [Required]
        public int? BranchID { get; set; }
        public string BranchName { get; set; }
        [Required]
        public int? OriginID { get; set; }
        [Required]
        public int? DestinationID { get; set; }
        [Required]
        public int? TruckID { get; set; }
        public int? DriverID { get; set; }
        public long? TripID { get; set; }
        public long? GcSetID { get; set; }        
        public string OriginName { get; set; }
        public string DestinationName { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class EventTypeModel
    {
        public byte? EventTypeID { get; set; }
        public byte? DefaultNextEventTypeID { get; set; }
        public string EventTypeName { get; set; }
        public string DefaultNextEventTypeName { get; set; }
        public bool IsStationary { get; set; }
        public bool IsGcRelated { get; set; }
        public bool IsTripRelated { get; set; }
        public bool IsDriverRelated { get; set; }
        public bool IsDriverRequired { get; set; }
        public bool IsBlocking { get; set; }
        public int? DisplayOrder { get; set; }
        public string  EventStatusText { get; set; }
        public byte? LimitPostEvent { get; set; }
    }
}
