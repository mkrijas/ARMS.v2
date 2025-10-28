using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class EventService : IEventService
    {
        IDbService Iservice;

        public EventService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update an event
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
               new SqlParameter("@DefaultNextEventTypeID", model.NextEventTypeID),
               new SqlParameter("@GcSetID", model.GcSetID),
               new SqlParameter("@AcceptedKM", model.AcceptedKM),
               new SqlParameter("@OriginID", model.OriginID),
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@EventID", model.TruckEventID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to delete an event by its ID
        public int Delete(long? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Truck.Delete]", parameters);
        }

        // Helper method to map data record to EventModel
        private EventModel GetModel(IDataRecord dr)
        {
            return new EventModel
            {
                BranchID = dr.GetInt32("BranchID"),
                DestinationID = dr.GetInt32("DestinationID"),
                DriverID = dr.GetInt32("DriverID"),
                EventReading = dr.GetInt64("EventReading"),
                EventTime = dr.GetDateTime("EventTime"),
                EventTypeID = dr.GetByte("EventTypeID"),
                NextEventTypeID = dr.GetByte("DefaultNextEventTypeID"),
                GcSetID = dr.GetInt64("GcSetID"),
                OriginID = dr.GetInt32("OriginID"),
                TripID = dr.GetInt64("TripID"),
                TruckEventID = dr.GetInt64("EventID"),
                TruckID = dr.GetInt32("TruckID"),
                OriginName = dr.GetString("OriginName"),
                DestinationName = dr.GetString("DestinationName"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to select an event by its ID
        private EventTypeModel GetEventType(IDataRecord dr)
        {
            return new EventTypeModel()
            {
                DisplayOrder = dr.GetInt32("DisplayOrder"),
                EventStatusText = dr.GetString("EventStatusText"),
                EventTypeID = dr.GetByte("EventTypeID"),
                DefaultNextEventTypeID = dr.GetByte("DefaultNextEventTypeID"),
                EventTypeName = dr.GetString("EventTypeName"),
                IsDriverRelated = dr.GetBoolean("IsDriverRelated"),
                IsDriverRequired = dr.GetBoolean("IsDriverRequired"),
                IsGcRelated = dr.GetBoolean("IsGcRelated"),
                IsStationary = dr.GetBoolean("IsStationary"),
                IsTripRelated = dr.GetBoolean("IsTripRelated"),
                IsBlocking = dr.GetBoolean("IsBlocking"),
                LimitPostEvent = dr.GetByte("LimitPostEvent"),
            };
        }

        // Method to get the next possible event for a truck
        public EventModel SelectByID(long? EventID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@EventID", EventID),
               new SqlParameter("@Operation", "SelectByID"),
            };
            EventModel model = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select events by trip ID
        EventTypeModel IEventService.GetNextPossibleEvent(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@Operation", "NextPossibleEvent"),
            };
            EventTypeModel model = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetEventType(dr);
            }
            return model;
        }

        // Method to get the current event for a truck
        IEnumerable<EventModel> IEventService.SelectByTrip(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "SelectByTrip"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                yield return GetModel(dr);
            }

        }

        // Method to get the previous event for a truck
        EventModel IEventService.GetCurrentEvent(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@Operation", "CurrentEvent"),
            };
            EventModel model = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to get the next event for a truck 
        EventModel IEventService.GetPreviousEvent(long? EventID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@EventID", EventID),
               new SqlParameter("@Operation", "PreviousEvent"),
            };
            EventModel model = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to get all event types
        EventModel IEventService.GetNextEvent(long? EventID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@EventID", EventID),
               new SqlParameter("@Operation", "NextEvent"),
            };
            EventModel model = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to get a specific event type by its ID
        IEnumerable<EventTypeModel> IEventService.GetEventTypes()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Type.Select]", null))
            {
                yield return GetEventType(dr);
            }
        }

        // Method to get the previous event based on truck ID and event time
        EventTypeModel IEventService.GetEventType(int? EventTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@EventTypeID", EventTypeID),
            };
            EventTypeModel model = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Type.Select]", parameters))
            {
                model = GetEventType(dr);
            }
            return model;
        }


        ///////////////
        // Method to get the previous event for a truck based on truck ID and event time
        EventModel IEventService.GetPreviousEvent(int truckID, DateTime eventTime)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", truckID),
               new SqlParameter("@EventTime", eventTime),
               new SqlParameter("@Operation", "PreviousEventReadingAndTimeEvent"),
            };
            EventModel model = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Event.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public int MoveToNonOperating(byte? eventTypeId, int? truckId, string userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@EventTypeID", eventTypeId),
               new SqlParameter("@TruckID", truckId),
               new SqlParameter("@UserID", userId),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Operating.Update]", parameters);
        }

        ///////////////
    }
}
