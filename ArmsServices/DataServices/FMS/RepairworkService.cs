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

        public int Delete(int? RepairJobID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RepairJobID", RepairJobID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.RepairJob.Delete]", parameters);
        }

        public IEnumerable<RepairJobModel> Select(int? RepairJobID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RepairJobID", RepairJobID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJob.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public RepairJobModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RepairJobID", ID),
            };
            RepairJobModel model = new();

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJob.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public RepairJobModel Update(RepairJobModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RepairJobID", model.RepairJobID),
                new SqlParameter("@RepairJobTitle", model.RepairJobTitle),
               new SqlParameter("@Description", model.Description),
               new SqlParameter("@MechanicalHours", model.MechanicalHours),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.RepairJob.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private RepairJobModel GetModel(IDataRecord dr)
        {
            return new RepairJobModel
            {
                RepairJobID = dr.GetInt32("RepairJobID"),
                RepairJobTitle = dr.GetString("RepairJobTitle"),
                Description = dr.GetString("Description"),
                MechanicalHours = dr.GetDecimal("MechanicalHours"),
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