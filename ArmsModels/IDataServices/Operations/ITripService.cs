using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITripService
    {
        TripModel Update(TripModel model);
        int Delete(long? TripID, string UserID);
        TripModel Select(long? TripID);
        TripModel SelectByTripNumber(string TripNumber);
        int Cancel(long? TripID, string UserID);
        int CloseTrip(long? TripID, int? BranchID, string UserID);
        bool IsClosed(long? TripID);
        bool IsSettled(long? TripID);
        TripInfoModel GetTripInfo(long? TripID);
        IAsyncEnumerable<TripModel> SearchTrips(int? TruckID, int? BranchID, string TripNumberSearchString);
        IEnumerable<object> GetOutstandingBills(long? TripID);
        IEnumerable<GcTariffModel> GetTariffs(long? TripID);
    }
}