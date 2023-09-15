using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class MechanicService : IMechanicService
    {
        IDbService Iservice;

        public MechanicService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? MechanicID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MechanicID", MechanicID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Workshop.Mechanic.Delete]", parameters);
        }

        public IEnumerable<MechanicModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "All"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Workshop.Mechanic.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public MechanicModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MechanicID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            MechanicModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Workshop.Mechanic.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<MechanicModel> SelectByWorkshop(int? WorkshopID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@WorkshopID", WorkshopID),
               new SqlParameter("@Operation", "ByWorkshop"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Workshop.Mechanic.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public MechanicModel Update(MechanicModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MechanicID", model.MechanicID),
               new SqlParameter("@MechanicName", model.MechanicName),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@WorkshopID", model.WorkshopID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Workshop.Mechanic.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private MechanicModel GetModel(IDataRecord dr)
        {
            return new MechanicModel
            {
                MechanicID = dr.GetInt32("MechanicID"),
                MechanicName = dr.GetString("MechanicName"),
                Remarks = dr.GetString("Remarks"),
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
