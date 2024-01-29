using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITripAdvanceService
    {
        TripAdvanceModel Update(TripAdvanceModel model);
        int Delete(int? TripAdvanceID, string UserID);
        TripAdvanceModel Select(int? TripAdvanceID);
        IEnumerable<TripAdvanceModel> SelectByTrip(long? TripID);
        TripAdvanceModel GetTotal(long? TripID);
    }
}