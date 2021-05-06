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
        BankAccountModel Update(BankAccountModel model);
        int Delete(int BankAccountID, string UserID);
        IEnumerable<BankAccountModel> Select(int? BankAccountID);
    }

    public class BankAccountService : IBankAccountService
    {
        IDbService Iservice;

        public BankAccountService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public BankAccountModel Update(BankAccountModel model)
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

            BankAccountModel rmodel = new BankAccountModel();
            using (var reader = Iservice.GetDataReader("[usp.Entity.BankAccountsUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new BankAccountModel
                    {
                        BankAccountID = reader.GetInt32("BankAccountID"),
                        AccountNumber = reader.GetString("AccountNumber"),
                        BeneficiaryName = reader.GetString("BeneficiaryName"),
                        GstID = reader.GetInt32("GstID"),
                        IfscCode = reader.GetString("IfscCode"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return rmodel;
        }
        public int Delete(int BankAccountID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankAccountID", BankAccountID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Entity.BankAccountsDelete]", parameters);
        }
        public IEnumerable<BankAccountModel> Select(int? BankAccountID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankAccountID", BankAccountID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Entity.BankAccountsSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new BankAccountModel
                    {
                        BankAccountID = reader.GetInt32("BankAccountID"),
                        AccountNumber = reader.GetString("AccountNumber"),
                        BeneficiaryName = reader.GetString("BeneficiaryName"),
                        GstID = reader.GetInt32("GstID"),
                        IfscCode = reader.GetString("IfscCode"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }

    }
}
