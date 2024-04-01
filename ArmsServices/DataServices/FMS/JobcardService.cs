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
               new SqlParameter("@Operation", "ByID"),
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
               new SqlParameter("@Operation", "ByBranchID"),
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
               new SqlParameter("@Operation", "ByID"),
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
               new SqlParameter("@Operation", "ByTruckID"),
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
               new SqlParameter("@Odometer", model.Odometer),
               new SqlParameter("@TripID", model.TripID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<JobcardModel> SelectByBranchAndTruck(int? BranchID, int? TruckID, bool Active = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranchIDAndTruckID"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@Active",Active)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Select]", parameters))
            {
                yield return GetModel(dr);
            }
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
                Odometer = (decimal)dr.GetDecimal("Odometer"),
                workshop = dr.GetString("WorkshopName"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<JobcardModel> SelectByJobCardID(int? JobCardID, bool Active = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByJobCardID"),
               new SqlParameter("@JobCardID", JobCardID),
               new SqlParameter("@Active",Active)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<JobInProgressModel> GetJobListByJobCardID(int? JobCardID)
        {
            bool Scrap = false;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "GetJobsByJobCardID"),
                new SqlParameter("@JobCardID", JobCardID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.Select]", parameters))
            {
                yield return GetModelJob(dr);
            }
        }

        private JobInProgressModel GetModelJob(IDataRecord dr)
        {
            return new JobInProgressModel
            {
                RepairJobTitle = dr.GetString("RepairJobTitle"),
                CreatedOn = dr.GetDateTime("CreatedOn"),
                FinishedOn = dr.GetDateTime("FinishedOn"),
                TimeTaken = dr.GetInt32("TimeTaken"),
                Mechanic = dr.GetString("Mechanic")
            };
        }

        public int? CloseJobcard(int? JobcardID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", JobcardID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Jobcard.Close]", parameters);
        }
    }
}
