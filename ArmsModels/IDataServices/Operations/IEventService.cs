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
        EventModel SelectByID(long? EventID);
        EventModel Update(EventModel model);  //Edit
        int Delete(long? EventID, string UserID);  //Delete
        EventTypeModel GetNextPossibleEvent(int? TruckID);
        IEnumerable<EventModel> SelectByTrip(long? TripID);
        EventModel GetCurrentEvent(int? TruckID);
        EventModel GetPreviousEvent(long? EventID);

        ///////////////
        EventModel GetPreviousEvent(int truckID, DateTime eventTime);
        ///////////////

        EventModel GetNextEvent(long? EventID);
        IEnumerable<EventTypeModel> GetEventTypes();
        EventTypeModel GetEventType(int? EventTypeID);
    }
}