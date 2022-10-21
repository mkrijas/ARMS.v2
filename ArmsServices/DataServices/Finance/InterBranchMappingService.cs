using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IInterBranchMappingService
    {
        InterBranchMappingModel Update(InterBranchMappingModel model);
        int Delete(int? ID, string UserID);
        IEnumerable<InterBranchMappingModel> Select();
        InterBranchMappingModel SelectByID(int ID);
    }
    public class InterBranchMappingService : IInterBranchMappingService
    {
        IDbService Iservice;

        public InterBranchMappingService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public InterBranchMappingModel Update(InterBranchMappingModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@CreditBranchID", model.CreditBranchID),
               new SqlParameter("@DebitBranchID", model.DebitBranchID),
               new SqlParameter("@TransactionType", model.TransactionType),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Mapping.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.InterBranchAccount.Mapping.Delete]", parameters);
        }
        public IEnumerable<InterBranchMappingModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Mapping.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public InterBranchMappingModel SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankAccountID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            InterBranchMappingModel model = new InterBranchMappingModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Mapping.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private InterBranchMappingModel GetModel(IDataRecord dr)
        {
            return new InterBranchMappingModel()
            {
                ID = dr.GetInt32("ID"),
                CoaID = dr.GetInt32("CoaID"),
                DebitBranchID = dr.GetInt32("DebitBranchID"),
                CreditBranchID = dr.GetInt32("CreditBranchID"),
                TransactionType = dr.GetString("TransactionType"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                }
            };
        }
    }

    
}
