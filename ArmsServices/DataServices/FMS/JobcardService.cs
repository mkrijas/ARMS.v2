using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class JobcardService : IJobcardService
    {
        IDbService Iservice;

        public JobcardService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int AddPurchase(int? JobCardID, int? PID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", JobCardID),
               new SqlParameter("@PID", PID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Jobcard.AddPurchase]", parameters);
        }

        public int Delete(int? JobcardID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", JobcardID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Jobcard.Delete]", parameters);
        }

        public IEnumerable<JobcardModel> Select(int? JobcardID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", JobcardID),
               new SqlParameter("@Active",false)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<JobcardModel> SelectByBranch(int? BranchID, bool Active = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Active",Active)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public JobcardModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", ID),
               new SqlParameter("@Active",false)
            };
            JobcardModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<JobcardModel> SelectByTruck(int? TruckID, bool Active = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@Active",Active)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public JobcardModel Update(JobcardModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", model.JobcardID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@BreakdownID", model.BreakdownID),
               new SqlParameter("@CreatedOn", model.CreatedOn),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@PMIID", model.PMIID),
               new SqlParameter("@RecordStatus", model.UserInfo.RecordStatus),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private JobcardModel GetModel(IDataRecord dr)
        {
            return new JobcardModel
            {
                JobcardID = dr.GetInt32("JobcardID"),
                BranchID = dr.GetInt32("BranchID"),
                BranchName = dr.GetString("BranchName"),
                BreakdownID = dr.GetInt32("BreakdownID"),
                CreatedOn = dr.GetDateTime("CreatedOn"),
                JobcardNumber = dr.GetString("JobcardPrefix") + dr.GetInt32("JobcardNumber").ToString(),
                TruckID = dr.GetInt32("TruckID"),
                RegNo = dr.GetString("RegNo"),
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
