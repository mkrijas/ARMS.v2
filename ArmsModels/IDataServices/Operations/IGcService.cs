using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IGcService
    {
        GcSetModel Update(GcSetModel model);
        int Delete(long? GcID, string UserID);
        List<GcSetModel> Select(int? BranchID);
        List<GcSetModel> SelectByTrip(long? TripID);
        List<GcSetModel> SelectUnAssigned(int? BranchID);
        List<GcSetModel> SelectToUnload(long? TripID);
        List<GcSetModel> SelectToDispatch(long? TripID);
        List<GcSetModel> SelectPending(long? TripID);
        GcSetModel SelectByID(long? GcSetID);
        IEnumerable<GcTypeModel> SelectGcTypes();
        int AppendToTrip(long? TripID, long? GcSetID, string UserID);
        int BeginUnload(long? TripID, long? GcSetID);
        int RemoveFromTrip(long? GcSetID, long? TripID, string UserID);
        int UpdateEwayBill(EwayBillModel model);
        decimal? GetFreight(int? OrderID, int? RouteID, int? Axles, decimal? Qty);
    }
}