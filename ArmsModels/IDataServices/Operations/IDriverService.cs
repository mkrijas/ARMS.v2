using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IDriverService
    {
        DriverModel Update(DriverModel model);  //Edit
        int Delete(int? DriverID, string UserID);  //Delete

        //int Resign(int? DriverID, string Remarks);  
        IEnumerable<DriverModel> Select();
        IEnumerable<DriverModel> SelectByBranch(int BranchID);
        DriverModel SelectByID(int? DriverID);
        int UpdateBranch(int? DriverID, int? BranchID, bool availStatus, string UserID);
        IEnumerable<int> GetAssignedBranches(int? DriverID);
        DriverModel FindDriver(DriverModel model = null, DriverLicenceModel licence = null);
        int AvailabilityStatus(int? DriverID);
        int Join(int? DriverID, int? BranchID, DateTime? StartDate, string UserID);  //Edit
        int Resign(int? DriverID, string Remarks, string UserID);  //Resign
        DriverLeaveModel GetLastLeave(int? DriverID);
        int BeginLeave(DriverLeaveModel LeaveModel);  //AllotLeave
        int EndLeave(int? DriverID, string UserID);  //AllotLeave
        public int? GetAssignedTruck(int? DriverID);
        IEnumerable<DriverModel> GetDriverByAdhaarNo(string AdhaarNo);
        public string GetWorkPeriod(int? DriverID);
        public string RemoveDriverFromTruck(int? driverId, string UserId);  //RemoveDriver

    }
}