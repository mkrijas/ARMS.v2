using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITripFuelService
    {
        TripFuelModel Update(TripFuelModel model);  //Edit
        int Delete(long? TripFuelID, string UserID);  //Edit
        TripFuelModel Select(int? TripFuelID);
        IEnumerable<TripFuelModel> SelectByTrip(long? TripID);
        IEnumerable<TripFuelModel> SelectByAssetTransfer(int? AssetTransferID);
        TripFuelModel SelectByTaxPurchase(int? TaxPurchaseID);
        IEnumerable<TripFuelModel> SelectByTransfer(int? RequestApprovalHistoryID);


    }
}