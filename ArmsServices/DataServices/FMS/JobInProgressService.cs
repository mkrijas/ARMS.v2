using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IJobInProgressService
    {
        JobInProgressModel Update(JobInProgressModel model);
        JobInProgressModel SelectByID(int? ID);
        int Delete(int? JipID, string UserID);
        IEnumerable<JobInProgressModel> Select(int? JipID);
    }


    public class JobInProgressService : IJobInProgressService
    {
        IDbService Iservice;

        public JobInProgressService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? JipID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JipID", JipID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Jobcard.JobInProgress.Delete]", parameters);
        }

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

        public JobInProgressModel Update(JobInProgressModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JipID", model.JipID),
               new SqlParameter("@RepairJobID", model.RepairJobID),
               new SqlParameter("@JobCardID", model.JobCardID),
               new SqlParameter("@WorkshopID", model.WorkshopID),
               new SqlParameter("@CreatedOn", model.CreatedOn),
               new SqlParameter("@FinishedOn", model.FinishedOn),
               new SqlParameter("@JobStatus", model.JobStatus),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.JobInProgress.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private JobInProgressModel GetModel(IDataRecord dr)
        {
            return new JobInProgressModel
            {
                JipID = dr.GetInt32("JipID"),
                RepairJobID = dr.GetInt32("BranchID"),
                JobCardID = dr.GetInt32("JobCardID"),
                WorkshopID = dr.GetInt32("WorkshopID"),
                CreatedOn = dr.GetDateTime("CreatedOn"),
                FinishedOn = dr.GetDateTime("FinishedOn"),
                JobStatus = dr.GetInt32("JobStatus"),
                Remarks = dr.GetString("Remarks"),
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