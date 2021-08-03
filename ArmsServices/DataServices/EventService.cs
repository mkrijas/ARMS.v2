using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IEventService
    {      
        EventModel SelectByID(long EventID);
        EventModel Update(EventModel model);
        int Delete(long EventID, string UserID);
        EventTypeModel GetNextPossibleEvent(int TruckID);
        IEnumerable<EventModel> SelectByTrip(long TripID);
        EventModel GetCurrentEvent(int TruckID);
        EventModel GetPreviousEvent(long EventID);
        IEnumerable<EventTypeModel> GetEventTypes();

    }

    public class EventService : IEventService
    {
        IDbService Iservice;

        public EventService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public EventModel Update(EventModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DestinationID", model.DestinationID),
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@EventReading", model.EventReading),
               new SqlParameter("@EventTime", model.EventTime),
               new SqlParameter("@EventTypeID", model.EventTypeID),
               new SqlParameter("@GcSetID", model.GcSetID),
               new SqlParameter("@OriginID", model.OriginID),
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@TruckEventID", model.TruckEventID),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };           

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Update]", parameters))
            {               
                model = GetModel(dr);
            }
            return model;
        }
        public int Delete(long ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Truck.Delete]", parameters);
        }

        private EventModel GetModel(IDataRecord dr)
        {
            return new EventModel
            {
              BranchID = dr.GetInt32("BranchID"),
              DestinationID = dr.GetInt32("DestinationID"),
              DriverID = dr.GetInt32("DriverID"),
              EventReading = dr.GetInt64("EventReading"),
              EventTime = dr.GetDateTime("EventTime"),
              EventTypeID = dr.GetInt32("EventTypeID"),
              GcSetID= dr.GetInt64("GcSetID"),
              OriginID= dr.GetInt32("OriginID"),
              TripID= dr.GetInt64("TripID"),
              TruckEventID= dr.GetInt64("TruckEventID"),
              TruckID= dr.GetInt32("TruckID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        
        public EventModel SelectByID(long EventID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@EventID", EventID),
               new SqlParameter("@Operation", "SelectByID"),
            };
            EventModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        EventTypeModel IEventService.GetNextPossibleEvent(int TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@Operation", "NextPossibleEvent"),
            };
            EventTypeModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
               model =  new EventTypeModel()
                {
                    DisplayOrder = dr.GetInt32("DisplayOrder"),
                    EventStatusText = dr.GetString("EventStatusText"),
                    EventTypeID = dr.GetInt32("EventTypeID"),
                    EventTypeName = dr.GetString("EventTypeName"),
                    IsDriverRelated = dr.GetBoolean("IsDriverRelated"),
                    IsDriverRequired = dr.GetBoolean("IsDriverRequired"),
                    IsGcRelated = dr.GetBoolean("IsGcRelated"),
                    IsStationary = dr.GetBoolean("IsStationary"),
                    IsTripRelated = dr.GetBoolean("IsTripRelated"),
                };
            }
            return model;
        }

        IEnumerable<EventModel> IEventService.SelectByTrip(long TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "SelectByID"),
            };            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                yield return GetModel(dr);
            }
            
        }

        EventModel IEventService.GetCurrentEvent(int TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@Operation", "CurrentEvent"),
            };
            EventModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        EventModel IEventService.GetPreviousEvent(long EventID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@EventID", EventID),
               new SqlParameter("@Operation", "PreviousEvent"),
            };
            EventModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        IEnumerable<EventTypeModel> IEventService.GetEventTypes()
        {           
            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Type.Select]", null))
            {
                yield return new EventTypeModel()
                {
                    DisplayOrder = dr.GetInt32("DisplayOrder"),
                    EventStatusText = dr.GetString("EventStatusText"),
                    EventTypeID = dr.GetInt32("EventTypeID"),
                    EventTypeName = dr.GetString("EventTypeName"),
                    IsDriverRelated = dr.GetBoolean("IsDriverRelated"),
                    IsDriverRequired = dr.GetBoolean("IsDriverRequired"),
                    IsGcRelated = dr.GetBoolean("IsGcRelated"),
                    IsStationary = dr.GetBoolean("IsStationary"),
                    IsTripRelated = dr.GetBoolean("IsTripRelated"),
                };
            }
           
        }
    }
}
