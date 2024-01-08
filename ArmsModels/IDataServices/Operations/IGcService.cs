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
        GcSetModel Update(GcSetModel model);  //Edit
        int DeleteSet(long? id, string UserID);
        int UpdateUnloadingQuantity(GcSetModel model);  //Edit
        int Delete(long? GcID, string UserID);  //Delete
        List<GcSetModel> Select(int? BranchID);
        List<GcSetModel> SelectByTrip(long? TripID);
        List<GcSetModel> SelectUnAssigned(int? BranchID);
        List<GcSetModel> SelectedUnloadEvent(long? TripID);
        List<GcSetModel> SelectToUnload(long? TripID);
        List<GcSetModel> SelectToDispatch(long? TripID);
        List<GcSetModel> SelectPending(long? TripID);
        GcSetModel SelectByID(long? GcSetID);
        IEnumerable<GcTypeModel> SelectGcTypes();
        int AppendToTrip(long? TripID, long? GcSetID, string UserID);  //AppentToTrip
        int BeginUnload(long? TripID, long? GcSetID);
        int RemoveFromTrip(long? GcSetID, long? TripID, string UserID);  //AppentToTrip
        EwayBillModel UpdateEwayBill(EwayBillModel model);  //Edit
        decimal? GetPrimaryFreight(int? OrderID, int? RouteID, int? Axles, decimal? Qty, decimal? freight);
        //decimal? GetFreight(int? orderID, int? routeID, object value, decimal? billQuantity, decimal? freight);
    }
}