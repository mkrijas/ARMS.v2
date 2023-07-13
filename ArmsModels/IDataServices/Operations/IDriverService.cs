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
        DriverModel Update(DriverModel model);
        int Delete(int? DriverID, string UserID);
        int Resign(int? DriverID, string Remarks);
        IEnumerable<DriverModel> Select();
        IEnumerable<DriverModel> SelectByBranch(int BranchID);
        DriverModel SelectByID(int? DriverID);
        int UpdateBranch(int? DriverID, int? BranchID, bool availStatus, string userID);
        IEnumerable<int> GetAssignedBranches(int? DriverID);
        DriverModel FindDriver(DriverModel model = null, DriverLicenceModel licence = null);
        int AvailabilityStatus(int? DriverID);
        int Join(int? DriverID, int? BranchID, DateTime? StartDate, string UserID);
        int Resign(int? DriverID, string Remarks, string userID);
        DriverLeaveModel GetLastLeave(int? DriverID);
        int BeginLeave(DriverLeaveModel LeaveModel);
        int EndLeave(int? DriverID, string UserID);
        public int? GetAssignedTruck(int? DriverID);
        IEnumerable<DriverModel> GetDriverByAdhaarNo(string AdhaarNo);
        public string GetWorkPeriod(int? DriverID);
        public string RemoveDriverFromTruck(int? driverId, string userId);

    }
}