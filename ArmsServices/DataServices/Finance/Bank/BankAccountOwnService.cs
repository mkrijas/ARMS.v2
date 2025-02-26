using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    // Service class for managing owned bank accounts
    public class BankAccountOwnService : IBankAccountOwnService
    {
        IDbService Iservice;
        IAddressService _address;
        IBankAccountService _bank;
        IContactService _contact;
        IBankPostingGroupService _postingGroup;
        public BankAccountOwnService(IDbService iservice,IAddressService address, IBankAccountService bank, IContactService contact, IBankPostingGroupService postingGroup)
        {
            Iservice = iservice;
            _address = address;
            _bank = bank;
            _contact = contact;
            _postingGroup = postingGroup;
        }

        // Updates an owned bank account and returns the updated model
        public OwnBankModel Update(OwnBankModel model)
        {
            BankAccountModel bankAccount = _bank.Update(model.BankAccountInfo);
            AddressModel addressObj = _address.Update(model.AddressInfo);
            ContactModel contactObj = _contact.Update(model.ContactInfo);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BankAccountID", bankAccount.BankAccountID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@AddressID", addressObj.AddressID),               
               new SqlParameter("@ContactID", contactObj.ContactID),
               new SqlParameter("@BankCode", model.BankCode),
               new SqlParameter("@PostingGroupID", model.PostingGroup.ID),
               new SqlParameter("@IsRegistered", model.IsGstRegistered),
               new SqlParameter("@GstNo", model.GstNo),      
               new SqlParameter("@TanNo", model.TANNo),
            };
            
            foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Own.Update]", parameters))
            {
                model = GetModel(dr); 
            }
            return model;
        }

        // Deletes an owned bank account by its ID and returns the number of affected rows
        public int Delete(int? ID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Finance.BankAccount.Own.Delete]", parameters);
        }

        // Retrieves a list of owned bank accounts
        public IEnumerable<OwnBankModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@Operation", "ByID"),
            };

            foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Own.Select]", parameters))
            {              
                    yield return GetModel(dr);
            }
        }

        // Retrieves owned bank accounts for a specific branch
        public IEnumerable<OwnBankModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ByBranch"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Own.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Helper method to map data from the database to an OwnBankModel
        private OwnBankModel GetModel(IDataRecord dr)
        {
            return new OwnBankModel()
            {
                ID = dr.GetInt32("ID"),
                BankAccountInfo = _bank.SelectByID(dr.GetInt32("BankAccountID").Value),
                BankCode = dr.GetString("BankCode"),
                BranchID = dr.GetInt32("BranchID"),
                IsGstRegistered = dr.GetBoolean("IsGstRegistered"),
                GstNo = dr.GetString("GstNo"),
                TANNo = dr.GetString("TanNo"),
                AddressInfo = new AddressModel() { AddressID = dr.GetInt32("AddressID")},
                ContactInfo = new ContactModel() { ContactID = dr.GetInt32("ContactID")},
                PostingGroup = new BankPostingGroupModel() { ID = dr.GetInt32("PostingGroupID") },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Retrieves an owned bank account by its bank code
        public OwnBankModel SelectByCode(string BankCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankCode", BankCode),
               new SqlParameter("@Operation", "ByCode"),
            };

            OwnBankModel model = new OwnBankModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Own.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Retrieves an owned bank account by its ID
        public OwnBankModel SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            OwnBankModel model = new OwnBankModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Own.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Retrieves the COA ID for bank charges associated with a specific bank
        public int? GetBankChargeCoaID(int? BankID)
        {
            return _postingGroup.SelectByBank(BankID).BankCharges.CoaID;
        }

        // Retrieves the COA ID for a bank account associated with a specific bank
        public int? GetBankAccountCoaID(int? BankID)
        {
            return _postingGroup.SelectByBank(BankID).BankAccount.CoaID;
        }

        // Retrieves the COA ID for processing fees associated with a specific bank
        public int? GetProcessingFeeCoaID(int? BankID)
        {
            return _postingGroup.SelectByBank(BankID).ProcessingFee.CoaID;
        }
    }
}
