using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class JobcardWorkshopService : IJobcardWorkshopService
    {
        IDbService Iservice;

        public JobcardWorkshopService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? JwID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JwID", JwID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Jobcard.WorkshopEntry.Delete]", parameters);
        }

        public IEnumerable<JobcardWorkshopModel> Select(int? JwID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JwID", JwID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.WorkshopEntry.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public JobcardWorkshopModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JwID", ID),
            };
            JobcardWorkshopModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.WorkshopEntry.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<JobcardWorkshopModel> SelectByJobcard(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", ID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.WorkshopEntry.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public JobcardWorkshopModel Update(JobcardWorkshopModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JwID", model.JwID),
               new SqlParameter("@EnteredOn", model.EnteredOn),
               new SqlParameter("@ExitOn", model.ExitOn),
               new SqlParameter("@JobCardID", model.JobCardID),
               new SqlParameter("@WorkshopID", model.WorkshopID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.WorkshopEntry.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private JobcardWorkshopModel GetModel(IDataRecord dr)
        {
            return new JobcardWorkshopModel
            {
                JwID = dr.GetInt32("JwID"),
                EnteredOn = dr.GetDateTime("EnteredOn"),
                ExitOn = dr.GetDateTime("ExitOn"),
                JobCardID = dr.GetInt32("JobCardID"),
                WorkshopID = dr.GetInt32("WorkshopID"),
                WorkshopName = dr.GetString("WorkshopName"),
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
