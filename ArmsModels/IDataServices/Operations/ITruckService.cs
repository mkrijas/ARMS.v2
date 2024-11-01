using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Core.BaseModels.Operations.ROI;


namespace ArmsServices.DataServices
{
    public interface ITruckService
    {
        IEnumerable<ROITonnageModel> SelectBSType();
        TruckModel Update(TruckModel model); //edit
        int UpdateRegistration(TruckRegistrationModel model);  //edit
        int? ValidateRegistrationDate(TruckRegistrationModel model);
        int Delete(int? TruckID, string UserID);  //delete
        IEnumerable<TruckModel> Select(int? TruckID);
        IEnumerable<TruckModel> SelectByBranch(int? BranchID, string Filer, string HomeOrOperation = "Operation");
        TruckModel SelectByAsset(int? AssetID);
        TruckModel SelectByID(int? ID);
        TruckRegistrationModel GetRegistration(int? TruckID);
        IEnumerable<TruckRegistrationModel> GetRegistrationList(int? TruckID);
        TruckRegistrationModel GetRegistration(string RegNo);
        int Sold(int? TruckID, DateTime? SoldDate);  //sell
        int ChangeRegistration(TruckRegistrationModel model);  //edit
        int UpdateDriver(int? TruckID, int? DriverID, bool AssignedStatus, string UserID);  //assigndriver
        int? GetAssignedDriver(int? TruckID);
        long? GetCurrentTrip(int? TruckID);
        long? GetLastTrip(int? TruckID);
        IEnumerable<TruckStatusModel> GetTruckStatus(int? BranchID);
        IEnumerable<TruckModel> SelectAllByBranch(bool IsChecked, int? BranchID = null, string Filer = "All", string HomeOrOperation = "AllOperation");
        IEnumerable<TruckStatusModel> GetTruckStatusByEvent(int? BranchID, string SelectedValue);
    }
}