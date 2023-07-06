using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
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
            };
            
            foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.Own.Update]", parameters))
            {
                model = GetModel(dr); 
            }
            return model;
        }
        public int Delete(int? ID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Finance.BankAccount.Own.Delete]", parameters);
        }
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

        public int? GetBankChargeCoaID(int? BankID)
        {
            return _postingGroup.SelectByBank(BankID).BankCharges.CoaID;
        }
        public int? GetBankAccountCoaID(int? BankID)
        {
            return _postingGroup.SelectByBank(BankID).BankAccount.CoaID;
        }
        public int? GetProcessingFeeCoaID(int? BankID)
        {
            return _postingGroup.SelectByBank(BankID).ProcessingFee.CoaID;
        }
    }
}
