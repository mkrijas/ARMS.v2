using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IWorkshopService
    {
        WorkshopModel Update(WorkshopModel model);
        WorkshopModel SelectByID(int? ID);
        int Delete(int? WorkshopID, string UserID);
        IEnumerable<WorkshopModel> Select(int? WorkshopID);
    }


public class WorkshopService:IWorkshopService
    {
        IDbService Iservice; IAddressService Iaddress;

        public WorkshopService(IDbService iservice,IAddressService iaddress)
        {
            Iservice = iservice;
            Iaddress = iaddress;
        }

        public int Delete(int? WorkshopID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@WorkshopID", WorkshopID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Workshop.Delete]", parameters);
        }

        public IEnumerable<WorkshopModel> Select(int? WorkshopID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@WorkshopID", WorkshopID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Workshop.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public WorkshopModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@WorkshopID", ID),
            };
            WorkshopModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Workshop.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public WorkshopModel Update(WorkshopModel model)
        {            
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@WorkshopID", model.WorkshopID),
               new SqlParameter("@ContactNumber", model.ContactNumber),
               new SqlParameter("@PartyID", model.PartyID),
               new SqlParameter("@GstID", model.GstID),
               new SqlParameter("@WorkshopName", model.WorkshopName),
               new SqlParameter("@WorkshopType", model.WorkshopType),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Workshop.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private WorkshopModel GetModel(IDataRecord dr)
        {
            return new WorkshopModel
            {
                WorkshopID = dr.GetInt32("WorkshopID"),
                WorkshopName = dr.GetString("WorkshopName"),                
                WorkshopType = dr.GetString("WorkshopType"),
                ContactNumber = dr.GetString("ContactNumber"),
                PartyID = dr.GetInt32("PartyID"),
                GstID = dr.GetInt32("GstID"),
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
