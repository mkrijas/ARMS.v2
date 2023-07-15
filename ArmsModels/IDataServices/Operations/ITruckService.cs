using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITruckService
    {
        TruckModel Update(TruckModel model); //edit
        int UpdateRegistration(TruckRegistrationModel model);  //edit
        int? ValidateRegistrationDate(TruckRegistrationModel model);
        int Delete(int? TruckID, string UserID);  //delete
        IEnumerable<TruckModel> Select(int? TruckID);
        IEnumerable<TruckModel> SelectByBranch(int? BranchID, string Filer = "All");
        TruckModel SelectByID(int? ID);
        TruckRegistrationModel GetRegistration(int? TruckID);
        IEnumerable<TruckRegistrationModel> GetRegistrationList(int? TruckID);
        TruckRegistrationModel GetRegistration(string RegNo);
        int Sold(int? TruckID, DateTime? SoldDate);  //sell
        int ChangeRegistration(TruckRegistrationModel model);  //edit
        int UpdateDriver(int? TruckID, int? DriverID, bool AssignedStatus, string UserID);  //assigndriver
        int? GetAssignedDriver(int? TruckID);
        long? GetCurrentTrip(int? TruckID);
        IEnumerable<TruckStatusModel> GetTruckStatus(int? BranchID);
    }
}