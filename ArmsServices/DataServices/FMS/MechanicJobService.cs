using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IMechanicJobService
    {
        MechanicJobModel Update(MechanicJobModel model);
        MechanicJobModel SelectByID(int? ID);
        int Delete(int? MjID, string UserID);
        IEnumerable<MechanicJobModel> Select(int? MjID);
    }
  

public class MechanicJobService:IMechanicJobService
    {
        IDbService Iservice;

        public MechanicJobService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? MjID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MjID", MjID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Jobcard.JobInProgress.Mechanic.Delete]", parameters);
        }

        public IEnumerable<MechanicJobModel> Select(int? MjID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MjID", MjID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.JobInProgress.Mechanic.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public MechanicJobModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MjID", ID),
            };
            MechanicJobModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.JobInProgress.Mechanic.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public MechanicJobModel Update(MechanicJobModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MjID", model.MjID),
               new SqlParameter("@AssignedOn", model.AssignedOn),
               new SqlParameter("@EndedOn", model.EndedOn),
               new SqlParameter("@MechanicID", model.MechanicID),
               new SqlParameter("@JipID", model.JipID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Jobcard.JobInProgress.Mechanic.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private MechanicJobModel GetModel(IDataRecord dr)
        {
            return new MechanicJobModel
            {
                MjID = dr.GetInt32("MjID"),
                AssignedOn = dr.GetDateTime("AssignedOn"),
                EndedOn = dr.GetDateTime("EndedOn"),
                MechanicID = dr.GetInt32("MechanicID"),
                JipID = dr.GetInt32("JipID"),               
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
