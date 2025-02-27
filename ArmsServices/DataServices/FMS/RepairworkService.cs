using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class RepairWorkService : IRepairWorkService
    {
        IDbService Iservice;

        public RepairWorkService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a repair job by its ID
        public int Delete(int? RepairJobID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RepairJobID", RepairJobID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.RepairJob.Delete]", parameters);
        }

        // Method to select repair jobs based on various criteria
        public IEnumerable<RepairJobModel> SelectJob(int? RepairJobID, int? RepairJobGroupID, int? RepairJobSubGroupID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "JOBS"),
               new SqlParameter("@RepairJobID", RepairJobID),
               new SqlParameter("@RepairJobGroupID", RepairJobGroupID),
               new SqlParameter("@RepairJobSubGroupID", RepairJobSubGroupID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJob.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select repair job groups and subgroups
        public IEnumerable<RepairJobModel> SelectJobGroupAndSub()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@Operation", "GROUPANDSUB"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJob.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a repair job by its ID
        public RepairJobModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RepairJobID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJob.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to select all repair job groups
        public IEnumerable<RepairJobGroup> SelectGroup()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJobGroup.Select]", null))
            {
                yield return new RepairJobGroup()
                {
                    ID = dr.GetInt32("RepairJobGroupID"),
                    Title = dr.GetString("Title"),
                };
            }

        }

        // Method to select repair job subgroups by group ID
        public IEnumerable<RepairJobGroup> SelectSubGroup(int? GroupID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RepairJobGroupID", GroupID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJobSubGroup.Select]", parameters))
            {
                yield return new RepairJobGroup()
                {
                    ID = dr.GetInt32("RepairJobSubgroupID"),
                    Title = dr.GetString("Title"),
                    ParentID = dr.GetInt32("RepairJobGroupID")
                };
            }
        }

        // Method to update a repair job
        public RepairJobModel Update(RepairJobModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RepairJobID", model.RepairJobID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@MechanicalHours", model.MechanicalHours),
               new SqlParameter("@RepairJobGroupID", model.JobGroup.ID),
               new SqlParameter("@RepairJobSubgroupID", model.JobSubGroup.ID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJob.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to RepairJobModel
        private RepairJobModel GetModel(IDataRecord dr)
        {
            return new RepairJobModel
            {
                RepairJobID = dr.GetInt32("RepairJobID"),
                Title = dr.GetString("Title"),
                JobCode = dr.GetString("JobCode"),
                MechanicalHours = dr.GetDecimal("MechanicalHours"),
                JobGroup = new RepairJobGroup()
                {
                    ID = dr.GetInt32("RepairJobGroupID"),
                    Title = dr.GetString("GroupTitle"),
                },
                JobSubGroup = new RepairJobGroup()
                {
                    ID = dr.GetInt32("RepairJobSubGroupID"),
                    Title = dr.GetString("SubGroupTitle"),
                    ParentID = dr.GetInt32("RepairJobGroupID"),
                },
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