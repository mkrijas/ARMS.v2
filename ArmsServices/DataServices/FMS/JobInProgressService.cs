using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class JobInProgressService : IJobInProgressService
    {
        IDbService Iservice;

        public JobInProgressService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a job in progress entry by its ID
        public int Delete(int? JipID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JipID", JipID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Jobcard.JobInProgress.Delete]", parameters);
        }

        // Method to select job in progress entries by their ID
        public IEnumerable<JobInProgressModel> Select(int? JipID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JipID", JipID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.JobInProgress.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a job in progress entry by its IDc
        public JobInProgressModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JipID", ID),
            };
            JobInProgressModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.JobInProgress.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select job in progress entries by job card ID
        public IEnumerable<JobInProgressModel> SelectByJobcard(int? JobcardID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", JobcardID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.JobInProgress.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update a job in progress entry
        public JobInProgressModel Update(JobInProgressModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JipID", model.JipID),
               new SqlParameter("@DriverFaultID",model.DriverFaultID),
               new SqlParameter("@RepairJobID", model.RepairJobID),
               new SqlParameter("@JobCardID", model.JobCardID),
               new SqlParameter("@WorkshopID", model.WorkshopID),
               new SqlParameter("@CreatedOn", model.CreatedOn),
               new SqlParameter("@FinishedOn", model.FinishedOn),
               new SqlParameter("@JobStatus", model.JobStatus),
               new SqlParameter("@WarrantyCheck",model.WarrantyCheck),
               new SqlParameter("@WarrantyExpiryDate",model.WarrantyExpiryDate),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@Odometer", model.Odometer),
               new SqlParameter("@TotalAmount", model.TotalAmount)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.JobInProgress.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to JobInProgressModel
        private JobInProgressModel GetModel(IDataRecord dr)
        {
            return new JobInProgressModel
            {
                JipID = dr.GetInt32("JipID"),
                DriverFaultID = dr.GetInt32("DriverFaultID"),
                RepairJobID = dr.GetInt32("RepairJobID"),
                RepairJobTitle = dr.GetString("RepairJobTitle"),
                JobCardID = dr.GetInt32("JobCardID"),
                WorkshopID = dr.GetInt32("WorkshopID"),
                CreatedOn = dr.GetDateTime("CreatedOn"),
                FinishedOn = dr.GetDateTime("FinishedOn"),
                JobStatus = dr.GetInt32("JobStatus"),
                WarrantyCheck = dr.GetBoolean("IsUnderWarranty"),
                WarrantyExpiryDate = dr.GetDateTime("WarrantyExpiryDate"),
                Remarks = dr.GetString("Remarks"),
                TotalAmount = dr.GetDecimal("Amount"),
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