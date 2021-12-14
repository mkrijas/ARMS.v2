using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IInsuranceClaimService
    {
        InsuranceClaimModel Update(InsuranceClaimModel model);
        InsuranceClaimModel SelectByID(int? ID);
        int Delete(int? InsuranceClaimID, string UserID);
        IEnumerable<InsuranceClaimModel> Select(int? ConsigneeID);
    }

    public class InsuranceClaimService : IInsuranceClaimService
    {
        IDbService Iservice;

        public InsuranceClaimService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? InsuranceClaimID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", InsuranceClaimID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Delete]", parameters);
        }

        public IEnumerable<InsuranceClaimModel> Select(int? InsuranceClaimID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", InsuranceClaimID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InsuranceClaimModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", ID),
            };
            InsuranceClaimModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public InsuranceClaimModel Update(InsuranceClaimModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InsuranceClaimID", model.InsuranceClaimID),
               new SqlParameter("@Images", string.Join(";",model.Images)),
               new SqlParameter("@BreakdownID", model.BreakdownID),
               new SqlParameter("@InsuranceID", model.InsuranceID),
               new SqlParameter("@IsOpen", model.IsOpen),            
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.InsuranceClaim.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private InsuranceClaimModel GetModel(IDataRecord dr)
        {
            return new InsuranceClaimModel
            {
                InsuranceClaimID = dr.GetInt32("InsuranceClaimID"),
                Images = dr.GetString("Images").Split(";").ToList(),
                BreakdownID = dr.GetInt32("BreakdownID"),
                InsuranceID = dr.GetInt32("InsuranceID"),
                IsOpen = dr.GetBoolean("IsOpen"),                
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
