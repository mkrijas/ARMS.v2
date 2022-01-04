using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IBreakdownService
    {
        BreakdownModel Update(BreakdownModel model);
        BreakdownModel SelectByID(int? ID);
        int Delete(int? BreakdownID, string UserID);
        IEnumerable<BreakdownModel> Select();
        IEnumerable<BreakdownModel> SelectPending(int BranchID);
    }

    public class BreakdownService : IBreakdownService
    {
        IDbService Iservice;

        public BreakdownService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? BreakdownID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", BreakdownID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Delete]", parameters);
        }

        public IEnumerable<BreakdownModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "All")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }


        public IEnumerable<BreakdownModel> SelectPending(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Pending"),
               new SqlParameter("@BranchID", BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public BreakdownModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            BreakdownModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public BreakdownModel Update(BreakdownModel model)
        {    
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", model.BreakdownID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@BreakdownTime", model.BreakdownTime),
               new SqlParameter("@BreakdownType", model.BreakdownType),
               new SqlParameter("@ContactNumber", model.ContactNumber),
               new SqlParameter("@Detail", model.Detail),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Update]", parameters))
            {
                model =  GetModel(dr);
            }
            return model;
        }

        private BreakdownModel GetModel(IDataRecord dr)
        {
            return new BreakdownModel
            {
                BreakdownID = dr.GetInt32("BreakdownID"),
                BranchID = dr.GetInt32("BranchID"),
                BreakdownTime = dr.GetDateTime("BreakdownTime"),
                BreakdownType = dr.GetString("BreakdownType"),
                ContactNumber = dr.GetString("ContactNumber"),
                Detail = dr.GetString("Detail"),
                TruckID = dr.GetInt32("TruckID"),
                RegNo = dr.GetString("RegNo"),
                IsClaimInitiated = dr.GetBoolean("IsClaimInitiated"),
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
   

     