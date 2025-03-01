using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;
namespace ArmsServices.DataServices
{
    public class DriverLeaveService : IDriverLeaveService
    {
        IDbService Iservice;
        public DriverLeaveService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a driver leave by its ID
        public int Delete(int? LeaveID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LeaveID", LeaveID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Leave.Delete]", parameters);
        }

        // Method to select driver leaves by driver ID
        public IEnumerable<DriverLeaveModel> Select(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Leave.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a driver leave by its ID
        public DriverLeaveModel SelectByID(int? LeaveID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LeaveID", LeaveID)
            };

            DriverLeaveModel model = new DriverLeaveModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Leave.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to update a driver's leave details
        public DriverLeaveModel Update(DriverLeaveModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@DriverID", model.Driver.DriverID),
               new SqlParameter("@LeaveID", model.LeaveID),
               new SqlParameter("@StartTime", model.StartTime),
               new SqlParameter("@EndTime", model.EndTime),
               new SqlParameter("@ExpectedReturn", model.ExpectedReturn),
               new SqlParameter("@Reason", model.Reason),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Leave.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to DriverLeaveModel
        private DriverLeaveModel GetModel(IDataRecord dr)
        {
            return new DriverLeaveModel
            {
                LeaveID = dr.GetInt32("LeaveID"),
                StartTime = dr.GetDateTime("StartTime"),
                EndTime = dr.GetDateTime("EndTime"),
                ExpectedReturn = dr.GetDateTime("ExpectedReturn"),
                Reason = dr.GetString("Reason"),
                Driver = new DriverModel()
                {
                    DriverID = dr.GetInt32("DriverID"),
                    DriverName = dr.GetString("DriverName"),
                },
                BranchID = dr.GetInt32("BranchID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }
    }
}