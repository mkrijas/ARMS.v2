using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class BankAccountService : IBankAccountService
    {
        IDbService Iservice;

        public BankAccountService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update a bank account record
        public BankAccountModel Update(BankAccountModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankAccountID", model.BankAccountID),
               new SqlParameter("@AccountNumber", model.AccountNumber),
               new SqlParameter("@BeneficiaryName", model.BeneficiaryName),               
               new SqlParameter("@IfscCode", model.IfscCode),
               new SqlParameter("@BankTitle", model.BankTitle),
               new SqlParameter("@BankBranch", model.BankBranch),
               new SqlParameter("@MicrCode", model.MicrCode),
               new SqlParameter("@SwiftCode", model.SwiftCode),
               new SqlParameter("@UserID", model.UserInfo.UserID),               
            };
            
            foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Update]", parameters))
            {
                model = GetModel(dr); 
            }
            return model;
        }

        // Method to delete a bank account record based on BankAccountID and UserID
        public int Delete(int? BankAccountID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankAccountID", BankAccountID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Finance.BankAccount.Delete]", parameters);
        }

        // Method to retrieve all bank account records
        public IEnumerable<BankAccountModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@Operation", "ByID"),
            };

            foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Select]", parameters))
            {              
                    yield return GetModel(dr);
            }
        }

        // Helper method to map database record to BankAccountModel
        private BankAccountModel GetModel(IDataRecord dr)
        {
            return new BankAccountModel()
            {
                BankAccountID = dr.GetInt32("BankAccountID"),
                AccountNumber = dr.GetString("AccountNumber"),
                BeneficiaryName = dr.GetString("BeneficiaryName"),
                IfscCode = dr.GetString("IfscCode"),
                BankBranch = dr.GetString("BankBranch"),
                BankTitle = dr.GetString("BankTitle"),
                MicrCode = dr.GetString("MicrCode"),
                SwiftCode = dr.GetString("SwiftCode"),                
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to retrieve bank account information based on PartyBranchID
        public BankAccountModel SelectByPartyBranch(int PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyBranchID", PartyBranchID),
               new SqlParameter("@Operation", "ByParty"),
            };

            BankAccountModel model = new BankAccountModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to retrieve bank account details based on a specific ID
        public BankAccountModel SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankAccountID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            BankAccountModel model = new BankAccountModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
    }
}
