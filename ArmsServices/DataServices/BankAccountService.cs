using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IBankAccountService
    {       
        Task<bool> Update(BankAccountModel model);
        Task<int> Delete(int BankAccountID, string UserID);
        IAsyncEnumerable<BankAccountModel> Select(int? BankAccountID);
    }

    public class BankAccountService : IBankAccountService
    {
        IDbService Iservice;

        public BankAccountService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public async Task<bool> Update(BankAccountModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BankAccountID", model.BankAccountID),
               new SqlParameter("@AccountNumber", model.AccountNumber),
               new SqlParameter("@BeneficiaryName", model.BeneficiaryName),
               new SqlParameter("@GstID", model.GstID),
               new SqlParameter("@IfscCode", model.IfscCode),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            
           await foreach(IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Entity.BankAccountsUpdate]", parameters))
            {                
                    model = new BankAccountModel
                    {
                        BankAccountID = dr.GetInt32("BankAccountID"),
                        AccountNumber = dr.GetString("AccountNumber"),
                        BeneficiaryName = dr.GetString("BeneficiaryName"),
                        GstID = dr.GetInt32("GstID"),
                        IfscCode = dr.GetString("IfscCode"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = dr.GetByte("RecordStatus"),
                            TimeStampField = dr.GetDateTime("TimeStamp"),
                            UserID = dr.GetString("UserID"),
                        },
                    };              
            }
            return true;
        }
        public async Task <int> Delete(int BankAccountID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankAccountID", BankAccountID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQueryAsync("[usp.Entity.BankAccountsDelete]", parameters);
        }
        public async IAsyncEnumerable<BankAccountModel> Select(int? BankAccountID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankAccountID", BankAccountID)               
            };

            await foreach(IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Entity.BankAccountsSelect]", parameters))
            {              
                    yield return new BankAccountModel
                    {
                        BankAccountID = dr.GetInt32("BankAccountID"),
                        AccountNumber = dr.GetString("AccountNumber"),
                        BeneficiaryName = dr.GetString("BeneficiaryName"),
                        GstID = dr.GetInt32("GstID"),
                        IfscCode = dr.GetString("IfscCode"),
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
}
